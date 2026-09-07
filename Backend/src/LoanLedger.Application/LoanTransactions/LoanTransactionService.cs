using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Domain.Enums;
using LoanLedger.Domain.Services;

namespace LoanLedger.Application.LoanTransactions;

public class LoanTransactionService : ILoanTransactionService
{
private readonly ILoanTransactionRepository _transactionRepository;
private readonly ILoanRepository _loanRepository;
private readonly IContactTrustService _contactTrustService;
private readonly IUnitOfWork _unitOfWork;

public LoanTransactionService(
    ILoanTransactionRepository transactionRepository,
    ILoanRepository loanRepository,
    IContactTrustService contactTrustService,
    IUnitOfWork unitOfWork)
{
    _transactionRepository = transactionRepository;
    _loanRepository = loanRepository;
    _contactTrustService = contactTrustService;
    _unitOfWork = unitOfWork;
}

// =========================================================
// CREATE TRANSACTION
// =========================================================

public async Task<Guid> CreateAsync(
    Guid userId,
    CreateLoanTransactionRequest request)
{
    // =========================================================
    // BASIC VALIDATION
    // =========================================================

    if (request.Amount <= 0)
        throw new Exception(
            "Transaction amount must be greater than zero.");
	
	await _unitOfWork.BeginTransactionAsync();

	try
	{

    // =========================================================
    // FIND LOAN
    // =========================================================

    var loan =
        await _loanRepository.GetByIdAsync(request.LoanId);

    if (loan == null)
        throw new Exception("Loan not found.");

    // =========================================================
    // SECURITY
    // =========================================================

    if (loan.UserId != userId)
        throw new UnauthorizedAccessException(
            "You do not have access to this loan.");

    // =========================================================
    // CLOSED LOAN
    // =========================================================

    if (loan.IsClosed)
        throw new Exception(
            "This loan is already closed.");

    // =========================================================
    // DUPLICATE REFERENCE NUMBER
    // =========================================================

    if (!string.IsNullOrWhiteSpace(
        request.ReferenceNumber))
    {
        var existingTransactions =
            await _transactionRepository.GetAllAsync();

        var duplicate =
            existingTransactions.Any(t =>
                !string.IsNullOrWhiteSpace(
                    t.ReferenceNumber) &&
                t.ReferenceNumber.Equals(
                    request.ReferenceNumber,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicate)
            throw new Exception(
                "A transaction with this reference number already exists.");
    }

    // =========================================================
    // VALIDATE ADJUSTMENT
    // =========================================================

    if (request.TransactionType ==
            LoanTransactionType.Adjustment &&
        request.AdjustmentDirection == null)
    {
        throw new Exception(
            "Adjustment direction is required for an adjustment transaction.");
    }

    if (request.TransactionType !=
            LoanTransactionType.Adjustment)
    {
        request.AdjustmentDirection = null;
    }

    // =========================================================
    // VALIDATE WAIVER
    // =========================================================

    if (request.TransactionType ==
            LoanTransactionType.Waiver &&
        request.WaiverType == null)
    {
        throw new Exception(
            "Waiver type is required for a waiver transaction.");
    }

    if (request.TransactionType !=
            LoanTransactionType.Waiver)
    {
        request.WaiverType = null;
    }

    // =========================================================
    // CREATE TRANSACTION
    // =========================================================

    var transaction = new LoanTransaction
    {
        Id = Guid.NewGuid(),

        LoanId = request.LoanId,

        TransactionType =
            request.TransactionType,

        Amount =
            request.Amount,

        TransactionDate =
            request.TransactionDate.Kind ==
                DateTimeKind.Utc
                ? request.TransactionDate
                : request.TransactionDate.ToUniversalTime(),

        ReferenceNumber =
            request.ReferenceNumber,

        PaymentMethod =
            request.PaymentMethod,

        Description =
            request.Description,

        AdjustmentDirection =
            request.AdjustmentDirection,

        WaiverType =
            request.WaiverType,

        CreatedAt =
            DateTime.UtcNow
    };

    // =========================================================
    // CALCULATE BALANCE EFFECT
    // =========================================================

    var balanceEffect =
		LoanBalanceCalculator.GetBalanceEffect(
            request.TransactionType,
            request.Amount,
            request.AdjustmentDirection,
            request.WaiverType);

    var newBalance =
        loan.CurrentBalance + balanceEffect;

    // =========================================================
	// PREVENT REPAYMENT OVERPAYMENT
	// =========================================================

	if (request.TransactionType ==
		LoanTransactionType.Repayment &&
		request.Amount > loan.CurrentBalance)
	{
		throw new Exception(
			$"Repayment amount cannot exceed the outstanding balance of " +
			$"{loan.CurrentBalance:N2}.");
	}

	// =========================================================
	// PREVENT NEGATIVE BALANCE
	// =========================================================

	if (newBalance < 0)
	{
		throw new Exception(
			$"Transaction would make the loan balance negative. " +
			$"Current balance is {loan.CurrentBalance:N2}.");
	}

    // =========================================================
    // UPDATE LOAN BALANCE
    // =========================================================

    loan.CurrentBalance =
        newBalance;

    // =========================================================
    // AUTOMATIC LOAN STATUS
    // =========================================================

    if (loan.CurrentBalance == 0)
    {
        loan.IsClosed = true;
        loan.ClosedAt = DateTime.UtcNow;
    }

    // =========================================================
    // SAVE TRANSACTION
    // =========================================================

    await _transactionRepository.AddAsync(transaction);

		await _unitOfWork.SaveChangesAsync();

		await _contactTrustService.RefreshForLoanAsync(
			userId,
			loan.Id);

		await _unitOfWork.CommitTransactionAsync();

		return transaction.Id;
	}
	catch
	{
		await _unitOfWork.RollbackTransactionAsync();

		throw;
	}
}


// =========================================================
// GET ALL TRANSACTIONS FOR CURRENT USER
// =========================================================

public async Task<List<LoanTransactionResponse>> GetAllAsync(
    Guid userId)
{
    var transactions =
        await _transactionRepository
            .GetByUserIdAsync(userId);

    return transactions
        .Select(MapToResponse)
        .ToList();
}

// =========================================================
// GET TRANSACTION BY ID
// =========================================================

public async Task<LoanTransactionResponse?> GetByIdAsync(
    Guid userId,
    Guid transactionId)
{
    var transaction =
        await _transactionRepository
            .GetByIdAsync(transactionId);

    if (transaction == null)
        return null;

    var loan =
        await _loanRepository
            .GetByIdAsync(transaction.LoanId);

    if (loan == null)
        return null;

    // SECURITY
    if (loan.UserId != userId)
        return null;

    return MapToResponse(transaction);
}

// =========================================================
// GET TRANSACTIONS FOR A LOAN
// =========================================================

public async Task<List<LoanTransactionResponse>>
    GetTransactionsByLoanAsync(
        Guid userId,
        Guid loanId)
{
    var loan =
        await _loanRepository.GetByIdAsync(loanId);

    if (loan == null)
        throw new Exception("Loan not found.");

    // SECURITY
    if (loan.UserId != userId)
        throw new UnauthorizedAccessException(
            "You do not have access to this loan.");

    var transactions =
        await _transactionRepository
            .GetByLoanIdAsync(loanId);

    return transactions
        .Select(MapToResponse)
        .ToList();
}

// =========================================================
// DELETE TRANSACTION
// =========================================================

public async Task DeleteTransactionAsync(
    Guid userId,
    Guid transactionId)
{
    await _unitOfWork.BeginTransactionAsync();

    try
    {
        var transaction =
            await _transactionRepository
                .GetByIdAsync(transactionId);

        if (transaction == null)
            throw new Exception(
                "Transaction not found.");

        var loan =
            await _loanRepository
                .GetByIdAsync(transaction.LoanId);

        if (loan == null)
            throw new Exception(
                "Loan not found.");

        // =========================================================
        // SECURITY
        // =========================================================

        if (loan.UserId != userId)
            throw new UnauthorizedAccessException(
                "You do not have access to this transaction.");
		
		// =========================================================
		// CLOSED LOAN PROTECTION
		// =========================================================

		if (loan.IsClosed)
		{
			throw new Exception(
				"This loan is closed. Reopen the loan before editing its transactions.");
		}

        // =========================================================
        // REVERSE TRANSACTION EFFECT
        // =========================================================

        var balanceEffect =
            LoanBalanceCalculator.GetBalanceEffect(
                transaction.TransactionType,
                transaction.Amount,
                transaction.AdjustmentDirection,
                transaction.WaiverType);

        loan.CurrentBalance -= balanceEffect;

        // =========================================================
        // PREVENT NEGATIVE BALANCE
        // =========================================================

        if (loan.CurrentBalance < 0)
        {
            throw new Exception(
                "Deleting this transaction would result in a negative loan balance.");
        }

        // =========================================================
        // RECALCULATE LOAN STATUS
        // =========================================================

        if (loan.CurrentBalance == 0)
		{
			loan.IsClosed = true;
			loan.ClosedAt = DateTime.UtcNow;
		}
	
        // =========================================================
        // DELETE TRANSACTION
        // =========================================================

        _transactionRepository.Delete(transaction);

        // =========================================================
        // SAVE EVERYTHING AS ONE UNIT
        // =========================================================

        await _unitOfWork.SaveChangesAsync();

        // =========================================================
        // REFRESH CONTACT TRUST
        // =========================================================

        await _contactTrustService.RefreshForLoanAsync(
            userId,
            loan.Id);

        // =========================================================
        // COMMIT
        // =========================================================

        await _unitOfWork.CommitTransactionAsync();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();

        throw;
    }
}

// =========================================================
// UPDATE TRANSACTION
// =========================================================

public async Task<bool> UpdateAsync(
    Guid userId,
    Guid transactionId,
    UpdateLoanTransactionRequest request)
{
    if (request.Amount <= 0)
        throw new Exception(
            "Transaction amount must be greater than zero.");

    await _unitOfWork.BeginTransactionAsync();

    try
    {
        // =========================================================
        // FIND TRANSACTION
        // =========================================================

        var transaction =
            await _transactionRepository
                .GetByIdAsync(transactionId);

        if (transaction == null)
            return false;

        // =========================================================
        // FIND LOAN
        // =========================================================

        var loan =
            await _loanRepository
                .GetByIdAsync(transaction.LoanId);

        if (loan == null)
            throw new Exception(
                "Loan not found.");

        // =========================================================
        // SECURITY
        // =========================================================

        if (loan.UserId != userId)
            throw new UnauthorizedAccessException(
                "You do not have access to this transaction.");
				
		// =========================================================
		// CLOSED LOAN PROTECTION
		// =========================================================

		if (loan.IsClosed)
		{
			throw new Exception(
				"This loan is closed. Reopen the loan before editing its transactions.");
		}

        // =========================================================
        // VALIDATE ADJUSTMENT
        // =========================================================

        if (request.TransactionType ==
                LoanTransactionType.Adjustment &&
            request.AdjustmentDirection == null)
        {
            throw new Exception(
                "Adjustment direction is required for an adjustment transaction.");
        }

        if (request.TransactionType !=
                LoanTransactionType.Adjustment)
        {
            request.AdjustmentDirection = null;
        }

        // =========================================================
        // VALIDATE WAIVER
        // =========================================================

        if (request.TransactionType ==
                LoanTransactionType.Waiver &&
            request.WaiverType == null)
        {
            throw new Exception(
                "Waiver type is required for a waiver transaction.");
        }

        if (request.TransactionType !=
                LoanTransactionType.Waiver)
        {
            request.WaiverType = null;
        }

        // =========================================================
        // PREVENT DUPLICATE REFERENCE NUMBER
        // =========================================================

        if (!string.IsNullOrWhiteSpace(
            request.ReferenceNumber))
        {
            var existingTransactions =
                await _transactionRepository
                    .GetAllAsync();

            var duplicate =
                existingTransactions.Any(t =>
                    t.Id != transactionId &&
                    !string.IsNullOrWhiteSpace(
                        t.ReferenceNumber) &&
                    t.ReferenceNumber.Equals(
                        request.ReferenceNumber,
                        StringComparison.OrdinalIgnoreCase));

            if (duplicate)
            {
                throw new Exception(
                    "A transaction with this reference number already exists.");
            }
        }

        // =========================================================
        // STEP 1: REVERSE OLD TRANSACTION EFFECT
        // =========================================================

        var oldBalanceEffect =
            LoanBalanceCalculator.GetBalanceEffect(
                transaction.TransactionType,
                transaction.Amount,
                transaction.AdjustmentDirection,
                transaction.WaiverType);

        loan.CurrentBalance -= oldBalanceEffect;

        if (loan.CurrentBalance < 0)
        {
            throw new Exception(
                "Updating this transaction would result in a negative loan balance.");
        }

        // =========================================================
        // STEP 2: PREVENT REPAYMENT OVERPAYMENT
        // =========================================================

        if (request.TransactionType ==
                LoanTransactionType.Repayment &&
            request.Amount > loan.CurrentBalance)
        {
            throw new Exception(
                $"Repayment amount cannot exceed the outstanding balance of " +
                $"{loan.CurrentBalance:N2}.");
        }

        // =========================================================
        // STEP 3: APPLY NEW TRANSACTION EFFECT
        // =========================================================

        var newBalanceEffect =
            LoanBalanceCalculator.GetBalanceEffect(
                request.TransactionType,
                request.Amount,
                request.AdjustmentDirection,
                request.WaiverType);

        var newBalance =
            loan.CurrentBalance +
            newBalanceEffect;

        // =========================================================
        // PREVENT NEGATIVE BALANCE
        // =========================================================

        if (newBalance < 0)
        {
            throw new Exception(
                $"Transaction would make the loan balance negative. " +
                $"Current balance is {loan.CurrentBalance:N2}.");
        }

        loan.CurrentBalance = newBalance;

        // =========================================================
        // STEP 4: UPDATE TRANSACTION
        // =========================================================

        transaction.TransactionType =
            request.TransactionType;

        transaction.Amount =
            request.Amount;

        transaction.TransactionDate =
            request.TransactionDate.Kind ==
                DateTimeKind.Utc
                ? request.TransactionDate
                : request.TransactionDate.ToUniversalTime();

        transaction.ReferenceNumber =
            request.ReferenceNumber?.Trim() ??
            string.Empty;

        transaction.PaymentMethod =
            request.PaymentMethod;

        transaction.Description =
            request.Description?.Trim() ??
            string.Empty;

        transaction.AdjustmentDirection =
            request.AdjustmentDirection;

        transaction.WaiverType =
            request.WaiverType;

        // =========================================================
        // STEP 5: RECALCULATE LOAN STATUS
        // =========================================================

        if (loan.CurrentBalance == 0)
		{
			loan.IsClosed = true;
			loan.ClosedAt = DateTime.UtcNow;
		}

        // =========================================================
        // STEP 6: SAVE
        // =========================================================

        await _unitOfWork.SaveChangesAsync();

        // =========================================================
        // STEP 7: REFRESH CONTACT TRUST
        // =========================================================

        await _contactTrustService.RefreshForLoanAsync(
            userId,
            loan.Id);

        // =========================================================
        // STEP 8: COMMIT
        // =========================================================

        await _unitOfWork.CommitTransactionAsync();

        return true;
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();

        throw;
    }
}

// =========================================================
// MAP ENTITY TO RESPONSE
// =========================================================

private static LoanTransactionResponse MapToResponse(
    LoanTransaction transaction)
{
    return new LoanTransactionResponse
    {
        Id = transaction.Id,

        LoanId =
            transaction.LoanId,

        TransactionType =
            (int)transaction.TransactionType,

        TransactionTypeName =
            transaction.TransactionType.ToString(),

        Amount =
            transaction.Amount,

        TransactionDate =
            transaction.TransactionDate,

        ReferenceNumber =
            transaction.ReferenceNumber,

        PaymentMethod =
            (int)transaction.PaymentMethod,

        PaymentMethodName =
            transaction.PaymentMethod.ToString(),

        Description =
            transaction.Description,

        AdjustmentDirection =
            transaction.AdjustmentDirection.HasValue
                ? (int)transaction
                    .AdjustmentDirection.Value
                : null,

        AdjustmentDirectionName =
            transaction.AdjustmentDirection?
                .ToString(),

        WaiverType =
            transaction.WaiverType.HasValue
                ? (int)transaction
                    .WaiverType.Value
                : null,

        WaiverTypeName =
            transaction.WaiverType?
                .ToString(),

        CreatedAt =
            transaction.CreatedAt
    };
}

}
