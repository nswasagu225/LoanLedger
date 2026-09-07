using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Domain.Services;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Loans;

public class LoanService : ILoanService
{
private readonly ILoanRepository _loanRepository;
private readonly ILoanTransactionRepository _transactionRepository;
private readonly IWorkspaceRepository _workspaceRepository;
private readonly IContactRepository _contactRepository;
private readonly ICategoryRepository _categoryRepository;
private readonly IGuarantorRepository _guarantorRepository;
private readonly IItemRepository _itemRepository;

public LoanService(
    ILoanRepository loanRepository,
    ILoanTransactionRepository transactionRepository,
    IWorkspaceRepository workspaceRepository,
    IContactRepository contactRepository,
    ICategoryRepository categoryRepository,
    IGuarantorRepository guarantorRepository,
    IItemRepository itemRepository)
{
    _loanRepository = loanRepository;
    _transactionRepository = transactionRepository;
    _workspaceRepository = workspaceRepository;
    _contactRepository = contactRepository;
    _categoryRepository = categoryRepository;
    _guarantorRepository = guarantorRepository;
    _itemRepository = itemRepository;
}

public async Task<CreateLoanResponse> CreateAsync(
    Guid userId,
    CreateLoanRequest request)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    // =========================================================
    // VALIDATE TITLE
    // =========================================================

    if (string.IsNullOrWhiteSpace(request.Title))
    {
        throw new Exception(
            "Loan title is required.");
    }

    var title = request.Title.Trim();

    if (title.Length > 200)
    {
        throw new Exception(
            "Loan title cannot exceed 200 characters.");
    }

    // =========================================================
    // VALIDATE INTEREST RATE
    // =========================================================

    if (request.InterestRate.HasValue &&
		request.InterestRate.Value < 0)
	{
		throw new Exception(
			"Interest rate cannot be negative.");
	}

	if (request.InterestRate.HasValue &&
		request.InterestRate.Value > 100)
	{
		throw new Exception(
			"Interest rate cannot exceed 100%.");
	}

    // =========================================================
    // VALIDATE LOAN DATE
    // =========================================================

    if (request.LoanDate == default)
    {
        throw new Exception(
            "Loan date is required.");
    }

    if (request.LoanDate > DateTime.UtcNow)
    {
        throw new Exception(
            "Loan date cannot be in the future.");
    }

    // =========================================================
    // VALIDATE DUE DATE
    // =========================================================

    if (request.DueDate.HasValue &&
        request.DueDate.Value < request.LoanDate)
    {
        throw new Exception(
            "Due date cannot be earlier than the loan date.");
    }

    // =========================================================
    // VALIDATE REPAYMENT PLAN
    // =========================================================

    if (!string.IsNullOrWhiteSpace(request.RepaymentPlan) &&
        request.RepaymentPlan.Length > 2000)
    {
        throw new Exception(
            "Repayment plan cannot exceed 2000 characters.");
    }

    var repaymentPlan =
        string.IsNullOrWhiteSpace(request.RepaymentPlan)
            ? null
            : request.RepaymentPlan.Trim();

    // =========================================================
    // VALIDATE NOTES
    // =========================================================

    if (!string.IsNullOrWhiteSpace(request.Notes) &&
        request.Notes.Length > 5000)
    {
        throw new Exception(
            "Notes cannot exceed 5000 characters.");
    }

    var notes =
        string.IsNullOrWhiteSpace(request.Notes)
            ? null
            : request.Notes.Trim();

    // =========================================================
    // VALIDATE LOAN DIRECTION
    // =========================================================

    if (!Enum.IsDefined(
        typeof(LoanDirection),
        request.Direction))
    {
        throw new Exception(
            "A valid loan direction is required.");
    }

    // =========================================================
    // VALIDATE WORKSPACE
    // =========================================================

    if (!request.WorkspaceId.HasValue ||
        request.WorkspaceId.Value == Guid.Empty)
    {
        throw new Exception(
            "Workspace is required.");
    }

    var workspace =
        await _workspaceRepository.GetByIdAsync(
            request.WorkspaceId.Value);

    if (workspace == null)
    {
        throw new Exception(
            "Workspace not found.");
    }

    if (workspace.UserId != userId)
    {
        throw new UnauthorizedAccessException(
            "You do not have access to this workspace.");
    }

    // =========================================================
    // VALIDATE CONTACT
    // =========================================================

    var contact =
        await _contactRepository.GetByIdAsync(
            request.ContactId);

    if (contact == null)
    {
        throw new Exception(
            "Contact not found.");
    }

    if (contact.UserId != userId)
    {
        throw new UnauthorizedAccessException(
            "You do not have access to this contact.");
    }

    // =========================================================
    // VALIDATE LOAN CATEGORY
    // =========================================================

    var category =
        (await _categoryRepository.GetByUserAsync(userId))
            .FirstOrDefault(x =>
                x.Id == request.LoanCategoryId);

    if (category == null)
    {
        throw new Exception(
            "Loan category not found or does not belong to this user.");
    }

    // =========================================================
    // VALIDATE GUARANTOR
    // =========================================================

    if (request.GuarantorId.HasValue &&
        request.GuarantorId.Value != Guid.Empty)
    {
        var guarantor =
            await _guarantorRepository.GetByIdAsync(
                userId,
                request.GuarantorId.Value);

        if (guarantor == null)
        {
            throw new Exception(
                "Guarantor not found or does not belong to this user.");
        }
    }

    // =========================================================
    // VALIDATE ITEMS
    // =========================================================

    if (request.Items == null ||
        request.Items.Count == 0)
    {
        throw new Exception(
            "At least one loan item is required.");
    }

    decimal principalAmount = 0;

    // =========================================================
    // VALIDATE AND CALCULATE ITEMS
    // =========================================================

    foreach (var requestItem in request.Items)
    {
        var itemName =
            requestItem.ItemName?.Trim();

        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new Exception(
                "Loan item name is required.");
        }

        if (itemName.Length > 200)
        {
            throw new Exception(
                $"Loan item name cannot exceed 200 characters.");
        }

        // -----------------------------------------------------
        // VALIDATE CATALOG ITEM
        // -----------------------------------------------------

        if (requestItem.ItemId.HasValue &&
            requestItem.ItemId.Value != Guid.Empty)
        {
            var catalogItem =
                await _itemRepository.GetByIdAsync(
                    userId,
                    requestItem.ItemId.Value);

            if (catalogItem == null)
            {
                throw new Exception(
                    $"Item '{itemName}' was not found in your item catalogue.");
            }
        }

        // -----------------------------------------------------
        // VALIDATE QUANTITY
        // -----------------------------------------------------

        if (requestItem.Quantity <= 0)
        {
            throw new Exception(
                $"Quantity must be greater than zero for item '{itemName}'.");
        }

        // -----------------------------------------------------
        // VALIDATE RATE
        // -----------------------------------------------------

        if (requestItem.Rate <= 0)
        {
            throw new Exception(
                $"Rate must be greater than zero for item '{itemName}'.");
        }

        // -----------------------------------------------------
        // CALCULATE ITEM AMOUNT
        // -----------------------------------------------------

        decimal amount;

        try
        {
            amount = checked(
                requestItem.Quantity *
                requestItem.Rate);
        }
        catch (OverflowException)
        {
            throw new Exception(
                $"The calculated amount for item '{itemName}' is too large.");
        }

        // -----------------------------------------------------
        // ADD TO PRINCIPAL
        // -----------------------------------------------------

        try
        {
            principalAmount = checked(
                principalAmount +
                amount);
        }
        catch (OverflowException)
        {
            throw new Exception(
                "The total loan amount is too large.");
        }
    }

    // =========================================================
    // CREATE LOAN
    // =========================================================

    var loan = new Loan
    {
        Id = Guid.NewGuid(),

        UserId = userId,

        ContactId = request.ContactId,

        LoanCategoryId = request.LoanCategoryId,

        WorkspaceId = request.WorkspaceId.Value,

        Direction = request.Direction,

        Title = title,

        PrincipalAmount = principalAmount,

        CurrentBalance = principalAmount,

		InterestRate = request.InterestRate ?? 0m,

        LoanDate =
            DateTime.SpecifyKind(
                request.LoanDate,
                DateTimeKind.Utc),

        DueDate =
            request.DueDate.HasValue
                ? DateTime.SpecifyKind(
                    request.DueDate.Value,
                    DateTimeKind.Utc)
                : null,

        GuarantorId =
            request.GuarantorId,

        RepaymentPlan =
            repaymentPlan,

        Notes =
            notes,

        IsClosed = false,

        ClosedAt = null
    };

    // =========================================================
    // CREATE LOAN ITEMS
    // =========================================================

    foreach (var requestItem in request.Items)
    {
        var itemName =
            requestItem.ItemName.Trim();

        var amount =
            requestItem.Quantity *
            requestItem.Rate;

        var loanItem = new LoanItem
        {
            Id = Guid.NewGuid(),

            LoanId = loan.Id,

            ItemId = requestItem.ItemId,

            ItemName = itemName,

            Unit =
                string.IsNullOrWhiteSpace(requestItem.Unit)
                    ? null
                    : requestItem.Unit.Trim(),

            Quantity = requestItem.Quantity,

            Rate = requestItem.Rate,

            Amount = amount,

            Notes =
                string.IsNullOrWhiteSpace(requestItem.Notes)
                    ? null
                    : requestItem.Notes.Trim()
        };

        loan.LoanItems.Add(loanItem);
    }

    // =========================================================
    // SAVE LOAN
    // =========================================================

    await _loanRepository.AddAsync(loan);

    await _loanRepository.SaveChangesAsync();

    // =========================================================
    // RETURN RESPONSE
    // =========================================================

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
    // =========================================================
    // VALIDATE USER
    // =========================================================

    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    // =========================================================
    // GET LOAN
    // =========================================================

    var loan =
        await _loanRepository.GetByIdAsync(
            loanId);

    if (loan == null)
    {
        return false;
    }

    // =========================================================
    // VALIDATE LOAN OWNERSHIP
    // =========================================================

    if (loan.UserId != userId)
    {
        return false;
    }

    // =========================================================
    // CLOSED LOAN PROTECTION
    // =========================================================

    if (loan.IsClosed)
    {
        throw new Exception(
            "This loan is closed. Reopen the loan before editing it.");
    }

    // =========================================================
    // VALIDATE TITLE
    // =========================================================

    if (string.IsNullOrWhiteSpace(request.Title))
    {
        throw new Exception(
            "Loan title is required.");
    }

    var title =
        request.Title.Trim();

    if (title.Length > 200)
    {
        throw new Exception(
            "Loan title cannot exceed 200 characters.");
    }

    // =========================================================
    // VALIDATE INTEREST RATE
    // =========================================================

    if (request.InterestRate.HasValue &&
		request.InterestRate.Value < 0)
	{
		throw new Exception(
			"Interest rate cannot be negative.");
	}

	if (request.InterestRate.HasValue &&
		request.InterestRate.Value > 100)
	{
		throw new Exception(
			"Interest rate cannot exceed 100%.");
	}

    // =========================================================
    // VALIDATE LOAN DATE
    // =========================================================

    DateTime loanDate = loan.LoanDate;

    if (request.LoanDate.HasValue)
    {
        if (request.LoanDate.Value > DateTime.UtcNow)
        {
            throw new Exception(
                "Loan date cannot be in the future.");
        }

        loanDate =
            DateTime.SpecifyKind(
                request.LoanDate.Value,
                DateTimeKind.Utc);
    }

    // =========================================================
    // VALIDATE DUE DATE
    // =========================================================

    if (request.DueDate.HasValue &&
        request.DueDate.Value < loanDate)
    {
        throw new Exception(
            "Due date cannot be earlier than the loan date.");
    }

    // =========================================================
    // VALIDATE NOTES
    // =========================================================

    if (!string.IsNullOrWhiteSpace(request.Notes) &&
        request.Notes.Length > 5000)
    {
        throw new Exception(
            "Notes cannot exceed 5000 characters.");
    }

    var notes =
        string.IsNullOrWhiteSpace(request.Notes)
            ? null
            : request.Notes.Trim();

    // =========================================================
    // VALIDATE CONTACT
    // =========================================================

    var contact =
        await _contactRepository.GetByIdAsync(
            request.ContactId);

    if (contact == null)
    {
        throw new Exception(
            "Contact not found.");
    }

    if (contact.UserId != userId)
    {
        throw new UnauthorizedAccessException(
            "You do not have access to this contact.");
    }

    // =========================================================
    // VALIDATE LOAN CATEGORY
    // =========================================================

    var category =
        (await _categoryRepository.GetByUserAsync(userId))
            .FirstOrDefault(x =>
                x.Id == request.LoanCategoryId);

    if (category == null)
    {
        throw new Exception(
            "Loan category not found or does not belong to this user.");
    }

    // =========================================================
    // UPDATE LOAN INFORMATION
    // =========================================================

    loan.ContactId =
        request.ContactId;

    loan.LoanCategoryId =
        request.LoanCategoryId;

    loan.Title =
        title;

    loan.InterestRate =
		request.InterestRate ?? 0m;

    loan.LoanDate =
        loanDate;

    loan.DueDate =
        request.DueDate.HasValue
            ? DateTime.SpecifyKind(
                request.DueDate.Value,
                DateTimeKind.Utc)
            : null;

    loan.Notes =
        notes;

    // =========================================================
    // SAVE CHANGES
    // =========================================================

    await _loanRepository.SaveChangesAsync();

    return true;
}

public async Task<bool> UpdateItemsAsync(
    Guid userId,
    Guid loanId,
    UpdateLoanItemsRequest request)
{
    var loan =
        await _loanRepository.GetByIdAsync(loanId);

    if (loan == null || loan.UserId != userId)
        return false;

    if (loan.IsClosed)
        throw new Exception(
            "This loan is already closed.");

    if (request == null ||
        request.Items == null ||
        request.Items.Count == 0)
    {
        throw new Exception(
            "At least one loan item is required.");
    }

    // =========================================================
    // VALIDATE ITEMS
    // =========================================================

    decimal principalAmount = 0;

    var newItems = new List<LoanItem>();

    foreach (var requestItem in request.Items)
	{
		var itemName =
			requestItem.ItemName?.Trim();

		if (string.IsNullOrWhiteSpace(itemName))
		{
			throw new Exception(
				"Loan item name is required.");
		}
		// ---------------------------------------------------------
		// VALIDATE CATALOG ITEM
		// ---------------------------------------------------------

		if (requestItem.ItemId.HasValue &&
			requestItem.ItemId.Value != Guid.Empty)
		{
			var catalogItem =
				await _itemRepository.GetByIdAsync(
					userId,
					requestItem.ItemId.Value);

			if (catalogItem == null)
			{
				throw new Exception(
					$"Item '{itemName}' was not found in your item catalogue.");
			}
		}

		if (requestItem.Quantity <= 0)
		{
			throw new Exception(
				$"Quantity must be greater than zero for item '{itemName}'.");
		}

		if (requestItem.Rate <= 0)
		{
			throw new Exception(
				$"Rate must be greater than zero for item '{itemName}'.");
		}

		decimal amount;

		try
		{
			amount =
				checked(
					requestItem.Quantity *
					requestItem.Rate);
		}
		catch (OverflowException)
		{
			throw new Exception(
				$"The calculated amount for item '{itemName}' is too large.");
		}

		try
		{
			principalAmount =
				checked(principalAmount + amount);
		}
		catch (OverflowException)
		{
			throw new Exception(
				"The total loan amount is too large.");
		}

		newItems.Add(new LoanItem
		{
			Id = Guid.NewGuid(),

			LoanId = loan.Id,

			ItemId = requestItem.ItemId,

			ItemName = itemName,

			Unit = requestItem.Unit?.Trim(),

			Quantity = requestItem.Quantity,

			Rate = requestItem.Rate,

			Amount = amount,

			Notes = requestItem.Notes?.Trim()
		});
	}

    loan.PrincipalAmount = principalAmount;

	// =========================================================
	// RECALCULATE BALANCE FROM TRANSACTIONS
	// =========================================================

	var transactions =
		await _transactionRepository.GetByLoanIdAsync(loan.Id);

	var totalEffects = transactions.Sum(t =>
		LoanBalanceCalculator.GetBalanceEffect(
			t.TransactionType,
			t.Amount,
			t.AdjustmentDirection,
			t.WaiverType));

	var recalculatedBalance =
		principalAmount + totalEffects;

	// =========================================================
	// PREVENT INVALID NEGATIVE BALANCE
	// =========================================================

	if (recalculatedBalance < 0)
	{
		throw new Exception(
			"Updating the loan items would result in a negative loan balance. " +
			"The new principal amount cannot be lower than the amount already " +
			"accounted for by the loan transactions.");
	}

	loan.CurrentBalance = recalculatedBalance;

	// =========================================================
	// AUTOMATIC LOAN STATUS
	// =========================================================

	if (loan.CurrentBalance == 0)
	{
		loan.IsClosed = true;
		loan.ClosedAt = DateTime.UtcNow;
	}
	else
	{
		loan.IsClosed = false;
		loan.ClosedAt = null;
	}

    // =========================================================
    // REPLACE ITEMS
    // =========================================================

    await _loanRepository.ReplaceItemsAsync(
        loan.Id,
        newItems);

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
		WorkspaceId = loan.WorkspaceId,
		ContactId = loan.ContactId,
		ContactName = loan.Contact?.FullName ?? string.Empty,
		LoanCategoryId = loan.LoanCategoryId,
		LoanCategory = loan.LoanCategory?.Name ?? string.Empty,

		Direction = (int)loan.Direction,
		DirectionName = loan.Direction.ToString(),

		Title = loan.Title,
		PrincipalAmount = loan.PrincipalAmount,
		CurrentBalance = loan.CurrentBalance,
		InterestRate = loan.InterestRate,
		LoanDate = loan.LoanDate,
		DueDate = loan.DueDate,
		Notes = loan.Notes ?? string.Empty,
		IsClosed = loan.IsClosed,
		ClosedAt = loan.ClosedAt,

		Items = loan.LoanItems
			.Select(item => new LoanItemDto
			{
				Id = item.Id,
				ItemId = item.ItemId,
				ItemName = item.ItemName,
				Unit = item.Unit,
				Quantity = item.Quantity,
				Rate = item.Rate,
				Amount = item.Amount,
				Notes = item.Notes
			})
			.ToList()
	}).ToList();
}
public async Task<List<LoanResponse>> GetByWorkspaceAsync(
    Guid userId,
    Guid workspaceId)
{
    // =========================================================
    // VALIDATE WORKSPACE
    // =========================================================

    var workspace =
        await _workspaceRepository
            .GetByIdAsync(workspaceId);

    if (workspace == null)
        throw new Exception(
            "Workspace not found.");

    if (workspace.UserId != userId)
        throw new UnauthorizedAccessException(
            "You do not have access to this workspace.");

    // =========================================================
    // GET LOANS
    // =========================================================

    var loans =
        await _loanRepository
            .GetByUserAndWorkspaceAsync(
                userId,
                workspaceId);

    // =========================================================
    // MAP RESPONSE
    // =========================================================

    return loans
        .Select(loan => new LoanResponse
        {
            Id = loan.Id,

            UserId = loan.UserId,

            WorkspaceId = loan.WorkspaceId,

            ContactId = loan.ContactId,

            ContactName =
                loan.Contact?.FullName ??
                string.Empty,

            LoanCategoryId =
                loan.LoanCategoryId,

            LoanCategory =
                loan.LoanCategory?.Name ??
                string.Empty,
				
			Direction = 
				(int)loan.Direction,
			
			DirectionName = 
				loan.Direction.ToString(),

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
                loan.Notes ?? string.Empty,

            IsClosed =
                loan.IsClosed,

            ClosedAt =
                loan.ClosedAt,

            Items =
                loan.LoanItems
                    .Select(item => new LoanItemDto
                    {
                        Id = item.Id,

                        ItemId = item.ItemId,

                        ItemName = item.ItemName,

                        Unit = item.Unit,

                        Quantity = item.Quantity,

                        Rate = item.Rate,

                        Amount = item.Amount,

                        Notes = item.Notes
                    })
                    .ToList()
        })
        .ToList();
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
		
		WorkspaceId = loan.WorkspaceId,

        ContactId = loan.ContactId,

        ContactName =
            loan.Contact?.FullName ??
            string.Empty,

        LoanCategoryId =
            loan.LoanCategoryId,

        LoanCategory =
            loan.LoanCategory?.Name ??
            string.Empty,
		Direction = 
			(int)loan.Direction,

		DirectionName = 
			loan.Direction.ToString(),

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
            loan.ClosedAt,

        Items =
            loan.LoanItems
                .Select(item => new LoanItemDto
                {
                    Id = item.Id,

                    ItemId = item.ItemId,

                    ItemName = item.ItemName,

                    Unit = item.Unit,

                    Quantity = item.Quantity,

                    Rate = item.Rate,

                    Amount = item.Amount,

                    Notes = item.Notes
                })
                .ToList()
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
		Direction = 
			(int)loan.Direction,

		DirectionName = 
			loan.Direction.ToString(),

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
		
		Direction = 
			(int)loan.Direction,

		DirectionName = 
			loan.Direction.ToString(),

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
    // =========================================================
    // VALIDATE USER
    // =========================================================

    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    // =========================================================
    // GET LOAN
    // =========================================================

    var loan =
        await _loanRepository.GetByIdAsync(
            loanId);

    if (loan == null ||
        loan.UserId != userId)
    {
        return false;
    }

    // =========================================================
    // CHECK CURRENT STATUS
    // =========================================================

    if (!loan.IsClosed)
    {
        throw new Exception(
            "This loan is already active.");
    }

    // =========================================================
    // REOPEN LOAN
    // =========================================================
    //
    // IMPORTANT:
    //
    // We do NOT modify CurrentBalance here.
    //
    // If the loan was closed while ₦40,000 was outstanding,
    // reopening simply makes that existing ₦40,000 active again.
    //
    // If the balance is already zero, reopening is still allowed
    // because manual open/close is controlled by the user.
    //
    // =========================================================

    loan.IsClosed = false;

    loan.ClosedAt = null;

    // =========================================================
    // SAVE
    // =========================================================

    await _loanRepository.SaveChangesAsync();

    return true;
}


public async Task<bool> CloseAsync(
    Guid userId,
    Guid loanId)
{
    // =========================================================
    // VALIDATE USER
    // =========================================================

    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    // =========================================================
    // GET LOAN
    // =========================================================

    var loan =
        await _loanRepository.GetByIdAsync(
            loanId);

    if (loan == null ||
        loan.UserId != userId)
    {
        return false;
    }

    // =========================================================
    // CHECK CURRENT STATUS
    // =========================================================

    if (loan.IsClosed)
    {
        throw new Exception(
            "This loan is already closed.");
    }

    // =========================================================
    // MANUAL CLOSE
    // =========================================================
    //
    // IMPORTANT:
    //
    // We intentionally DO NOT change:
    //
    //     PrincipalAmount
    //     CurrentBalance
    //
    // Therefore a loan may be closed while money is still
    // outstanding.
    //
    // Example:
    //
    // PrincipalAmount = ₦100,000
    // CurrentBalance  = ₦40,000
    //
    // After manual close:
    //
    // PrincipalAmount = ₦100,000
    // CurrentBalance  = ₦40,000
    // IsClosed        = true
    //
    // The ₦40,000 remains recorded against the loan.
    //
    // However, contact-level ACTIVE outstanding totals must
    // ignore this closed loan until it is reopened.
    //
    // =========================================================

    loan.IsClosed = true;

    loan.ClosedAt =
        DateTime.UtcNow;

    // =========================================================
    // SAVE
    // =========================================================

    await _loanRepository.SaveChangesAsync();

    return true;
}


}
