using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Loans;

public class LoanService : ILoanService
{
private readonly ILoanRepository _loanRepository;
private readonly ILoanTransactionRepository _transactionRepository;

public LoanService(
    ILoanRepository loanRepository,
    ILoanTransactionRepository transactionRepository)
{
    _loanRepository = loanRepository;
    _transactionRepository = transactionRepository;
}

public async Task<CreateLoanResponse> CreateAsync(
    Guid userId,
    CreateLoanRequest request)
{
    var loan = new Loan
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        ContactId = request.ContactId,
        LoanCategoryId = request.LoanCategoryId,
        Title = request.Title,
        PrincipalAmount = request.PrincipalAmount,
        CurrentBalance = request.PrincipalAmount,
        InterestRate = request.InterestRate,
        LoanDate = DateTime.SpecifyKind(request.LoanDate, DateTimeKind.Utc),
        DueDate = request.DueDate.HasValue
            ? DateTime.SpecifyKind(request.DueDate.Value, DateTimeKind.Utc)
            : null,
        Notes = request.Notes,
        IsClosed = false
    };

    await _loanRepository.AddAsync(loan);
    await _loanRepository.SaveChangesAsync();

    return new CreateLoanResponse
    {
        Success = true,
        Message = "Loan created successfully.",
        LoanId = loan.Id
    };
}

public async Task<bool> UpdateAsync(
    Guid userId,
    Guid loanId,
    UpdateLoanRequest request)
{
    var loan = await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return false;

    if (loan.IsClosed)
        throw new Exception("This loan is already closed.");

    if (request.InterestRate < 0)
        throw new Exception("Interest rate cannot be negative.");

    loan.ContactId = request.ContactId;
    loan.LoanCategoryId = request.LoanCategoryId;
    loan.Title = request.Title;
    loan.InterestRate = request.InterestRate;
    loan.DueDate = request.DueDate.HasValue
        ? DateTime.SpecifyKind(request.DueDate.Value, DateTimeKind.Utc)
        : null;
    loan.Notes = request.Notes ?? string.Empty;

    await _loanRepository.SaveChangesAsync();

    return true;
}

public async Task<List<LoanResponse>> GetByUserAsync(Guid userId)
{
    var loans = await _loanRepository.GetByUserAsync(userId);

    return loans.Select(loan => new LoanResponse
    {
        Id = loan.Id,
        UserId = loan.UserId,
        ContactId = loan.ContactId,
        ContactName = loan.Contact?.FullName ?? string.Empty,
        LoanCategoryId = loan.LoanCategoryId,
        LoanCategory = loan.LoanCategory?.Name ?? string.Empty,
        Title = loan.Title,
        PrincipalAmount = loan.PrincipalAmount,
        CurrentBalance = loan.CurrentBalance,
        InterestRate = loan.InterestRate,
        LoanDate = loan.LoanDate,
        DueDate = loan.DueDate,
        Notes = loan.Notes ?? string.Empty,
        IsClosed = loan.IsClosed,
        ClosedAt = loan.ClosedAt
    }).ToList();
}
public async Task<LoanResponse?> GetByIdAsync(
    Guid userId,
    Guid loanId)
{
    var loan =
        await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return null;

    return new LoanResponse
    {
        Id = loan.Id,

        UserId = loan.UserId,

        ContactId = loan.ContactId,

        ContactName =
            loan.Contact?.FullName ??
            string.Empty,

        LoanCategoryId =
            loan.LoanCategoryId,

        LoanCategory =
            loan.LoanCategory?.Name ??
            string.Empty,

        Title =
            loan.Title,

        PrincipalAmount =
            loan.PrincipalAmount,

        CurrentBalance =
            loan.CurrentBalance,

        InterestRate =
            loan.InterestRate,

        LoanDate =
            loan.LoanDate,

        DueDate =
            loan.DueDate,

        Notes =
            loan.Notes ??
            string.Empty,

        IsClosed =
            loan.IsClosed,

        ClosedAt =
            loan.ClosedAt
    };
}

public async Task<LoanSummaryResponse?> GetSummaryAsync(
    Guid userId,
    Guid loanId)
{
    var loan =
        await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return null;

    var transactions =
        await _transactionRepository
            .GetByLoanIdAsync(loanId);

    // =========================================================
    // TOTAL REPAYMENTS
    // =========================================================

    var totalRepaid = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Repayment)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL INTEREST CHARGED
    // =========================================================

    var totalInterestCharged = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Interest)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL INTEREST WAIVED
    // =========================================================

    var totalInterestWaived = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Waiver &&
            t.WaiverType ==
            Domain.Enums.WaiverType.Interest)
        .Sum(t => t.Amount);

    // =========================================================
    // NET INTEREST
    // =========================================================

    var totalInterest =
        Math.Max(
            0,
            totalInterestCharged -
            totalInterestWaived);

    // =========================================================
    // TOTAL PENALTY CHARGED
    // =========================================================

    var totalPenaltyCharged = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Penalty)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL PENALTY WAIVED
    // =========================================================

    var totalPenaltyWaived = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Waiver &&
            t.WaiverType ==
            Domain.Enums.WaiverType.Penalty)
        .Sum(t => t.Amount);

    // =========================================================
    // NET PENALTY
    // =========================================================

    var totalPenalty =
        Math.Max(
            0,
            totalPenaltyCharged -
            totalPenaltyWaived);

    // =========================================================
    // LAST REPAYMENT DATE
    // =========================================================

    var lastPaymentDate = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Repayment)
        .OrderByDescending(t => t.TransactionDate)
        .Select(t => (DateTime?)t.TransactionDate)
        .FirstOrDefault();

    // =========================================================
    // RESPONSE
    // =========================================================

    return new LoanSummaryResponse
    {
        LoanId =
            loan.Id,

        Title =
            loan.Title,

        ContactName =
            loan.Contact?.FullName ??
            string.Empty,

        LoanCategory =
            loan.LoanCategory?.Name ??
            string.Empty,

        PrincipalAmount =
            loan.PrincipalAmount,

        CurrentBalance =
            loan.CurrentBalance,

        TotalRepaid =
            totalRepaid,

        TotalInterest =
            totalInterest,

        TotalPenalty =
            totalPenalty,

        TransactionCount =
            transactions.Count,

        LastPaymentDate =
            lastPaymentDate,

        InterestRate =
            loan.InterestRate,

        LoanDate =
            loan.LoanDate,

        DueDate =
            loan.DueDate,

        Status =
            loan.IsClosed
                ? "Closed"
                : "Active"
    };
}

public async Task<LoanStatementResponse?> GetStatementAsync(
    Guid userId,
    Guid loanId)
{
    var loan = await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return null;

    var transactions =
        await _transactionRepository.GetByLoanIdAsync(loanId);

    // =========================================================
    // TOTAL REPAYMENTS
    // =========================================================

    var totalRepaid = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Repayment)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL INTEREST CHARGED
    // =========================================================

    var totalInterestCharged = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Interest)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL INTEREST WAIVED
    // =========================================================

    var totalInterestWaived = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Waiver &&
            t.WaiverType ==
            Domain.Enums.WaiverType.Interest)
        .Sum(t => t.Amount);

    // =========================================================
    // NET INTEREST
    // =========================================================

    var totalInterest =
        Math.Max(
            0,
            totalInterestCharged -
            totalInterestWaived);

    // =========================================================
    // TOTAL PENALTY CHARGED
    // =========================================================

    var totalPenaltyCharged = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Penalty)
        .Sum(t => t.Amount);

    // =========================================================
    // TOTAL PENALTY WAIVED
    // =========================================================

    var totalPenaltyWaived = transactions
        .Where(t =>
            t.TransactionType ==
            Domain.Enums.LoanTransactionType.Waiver &&
            t.WaiverType ==
            Domain.Enums.WaiverType.Penalty)
        .Sum(t => t.Amount);

    // =========================================================
    // NET PENALTY
    // =========================================================

    var totalPenalty =
        Math.Max(
            0,
            totalPenaltyCharged -
            totalPenaltyWaived);

    // =========================================================
    // RESPONSE
    // =========================================================

    return new LoanStatementResponse
    {
        LoanId = loan.Id,

        Title = loan.Title,

        ContactName =
            loan.Contact?.FullName ??
            string.Empty,

        LoanCategory =
            loan.LoanCategory?.Name ??
            string.Empty,

        PrincipalAmount =
            loan.PrincipalAmount,

        CurrentBalance =
            loan.CurrentBalance,

        TotalRepaid =
            totalRepaid,

        TotalInterest =
            totalInterest,

        TotalPenalty =
            totalPenalty,

        TransactionCount =
            transactions.Count,

        InterestRate =
            loan.InterestRate,

        LoanDate =
            loan.LoanDate,

        DueDate =
            loan.DueDate,

        Status =
            loan.IsClosed
                ? "Closed"
                : "Active",

        Transactions =
            transactions
                .OrderBy(t => t.TransactionDate)
                .ThenBy(t => t.CreatedAt)
                .Select(t => new LoanStatementTransaction
                {
                    Id =
                        t.Id,

                    TransactionType =
                        t.TransactionType.ToString(),

                    Amount =
                        t.Amount,

                    TransactionDate =
                        t.TransactionDate,

                    ReferenceNumber =
                        t.ReferenceNumber ??
                        string.Empty,

                    PaymentMethod =
                        t.PaymentMethod.ToString(),

                    Description =
                        t.Description ??
                        string.Empty
                })
                .ToList()
    };
}

public async Task<bool> ReopenAsync(
    Guid userId,
    Guid loanId)
{
    var loan = await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return false;

    if (!loan.IsClosed)
        throw new Exception("This loan is already active.");

    if (loan.CurrentBalance <= 0)
        throw new Exception("A fully repaid loan cannot be reopened.");

    loan.IsClosed = false;
    loan.ClosedAt = null;

    await _loanRepository.SaveChangesAsync();

    return true;
}
public async Task<bool> CloseAsync(
    Guid userId,
    Guid loanId)
{
    var loan =
        await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return false;

    if (loan.IsClosed)
        throw new Exception(
            "This loan is already closed.");

    if (loan.CurrentBalance > 0)
        throw new Exception(
            "A loan cannot be closed while it has an outstanding balance.");

    loan.IsClosed = true;
    loan.ClosedAt = DateTime.UtcNow;

    await _loanRepository.SaveChangesAsync();

    return true;
}


}
