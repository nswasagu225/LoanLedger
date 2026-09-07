using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Contacts;

public class ContactTrustService : IContactTrustService
{
    private readonly IContactRepository _contactRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IContactTrustRepository _trustRepository;
    private readonly ILoanRepository _loanRepository;

    public ContactTrustService(
        IContactRepository contactRepository,
        IWorkspaceRepository workspaceRepository,
        IContactTrustRepository trustRepository,
        ILoanRepository loanRepository)
    {
        _contactRepository = contactRepository;
        _workspaceRepository = workspaceRepository;
        _trustRepository = trustRepository;
        _loanRepository = loanRepository;
    }

    // =========================================================
    // GET OR CREATE TRUST PROFILE
    // =========================================================

    public async Task<ContactTrustDto> GetOrCreateAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId)
    {
        await ValidateOwnershipAsync(
            userId,
            contactId,
            workspaceId);

        var existing =
            await _trustRepository
                .GetByContactAndWorkspaceAsync(
                    contactId,
                    workspaceId);

        if (existing != null)
        {
            return MapToDto(existing);
        }

        var trust = new ContactTrust
        {
            Id = Guid.NewGuid(),

            ContactId = contactId,

            WorkspaceId = workspaceId,

            Score = 50,

            UpdatedAt = DateTime.UtcNow
        };

        await _trustRepository.AddAsync(trust);

        await _trustRepository.SaveChangesAsync();

        return MapToDto(trust);
    }

    // =========================================================
    // GET ALL TRUST PROFILES FOR CONTACT
    // =========================================================

    public async Task<List<ContactTrustDto>> GetByContactAsync(
        Guid userId,
        Guid contactId)
    {
        var contact =
            await _contactRepository
                .GetByIdAsync(contactId);

        if (contact == null)
            throw new Exception(
                "Contact not found.");

        if (contact.UserId != userId)
            throw new UnauthorizedAccessException(
                "You do not have access to this contact.");

        var trusts =
            await _trustRepository
                .GetByContactIdAsync(contactId);

        return trusts
            .Select(MapToDto)
            .ToList();
    }

    // =========================================================
    // REFRESH TRUST SCORE
    // =========================================================

    public async Task<ContactTrustDto> RefreshAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId)
    {
        await ValidateOwnershipAsync(
            userId,
            contactId,
            workspaceId);

        var trust =
            await _trustRepository
                .GetByContactAndWorkspaceAsync(
                    contactId,
                    workspaceId);

        if (trust == null)
        {
            trust = new ContactTrust
            {
                Id = Guid.NewGuid(),

                ContactId = contactId,

                WorkspaceId = workspaceId,

                Score = 50,

                UpdatedAt = DateTime.UtcNow
            };

            await _trustRepository.AddAsync(trust);
        }

        var score =
            await CalculateScoreAsync(
                userId,
                contactId,
                workspaceId);

        trust.Score = score;

        trust.UpdatedAt =
            DateTime.UtcNow;

        await _trustRepository
            .SaveChangesAsync();

        return MapToDto(trust);
    }

    // =========================================================
    // REFRESH TRUST AFTER LOAN CHANGE
    // =========================================================

    public async Task RefreshForLoanAsync(
        Guid userId,
        Guid loanId)
    {
        var loan =
            await _loanRepository
                .GetByIdAsync(loanId);

        if (loan == null)
            return;

        if (loan.UserId != userId)
            return;

        if (!loan.WorkspaceId.HasValue)
            return;

        await RefreshAsync(
            userId,
            loan.ContactId,
            loan.WorkspaceId.Value);
    }

    // =========================================================
    // CALCULATE SCORE
    // =========================================================

    private async Task<decimal> CalculateScoreAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId)
    {
        var loans =
            await _loanRepository
                .GetByUserAndWorkspaceAsync(
                    userId,
                    workspaceId);

        var contactLoans =
            loans
                .Where(x =>
                    x.ContactId == contactId)
                .ToList();

        decimal score = 50;

        foreach (var loan in contactLoans)
        {
            // -------------------------------------------------
            // CLOSED LOAN
            // -------------------------------------------------

            if (loan.IsClosed)
            {
                if (loan.DueDate.HasValue &&
                    loan.ClosedAt.HasValue)
                {
                    var dueDate =
                        loan.DueDate.Value.Date;

                    var closedDate =
                        loan.ClosedAt.Value.Date;

                    if (closedDate <= dueDate)
                    {
                        score += 15;
                    }
                    else
                    {
                        score += 5;
                    }
                }
                else
                {
                    // No due date means we cannot classify
                    // the repayment as early or late.
                    score += 5;
                }

                continue;
            }

            // -------------------------------------------------
            // ACTIVE LOAN
            // -------------------------------------------------

            if (loan.DueDate.HasValue &&
                DateTime.UtcNow.Date >
                loan.DueDate.Value.Date)
            {
                score -= 15;
            }
        }

        // -----------------------------------------------------
        // LIMIT SCORE
        // -----------------------------------------------------

        if (score > 100)
            score = 100;

        if (score < 0)
            score = 0;

        return score;
    }

    // =========================================================
    // VALIDATE OWNERSHIP
    // =========================================================

    private async Task ValidateOwnershipAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId)
    {
        var contact =
            await _contactRepository
                .GetByIdAsync(contactId);

        if (contact == null)
            throw new Exception(
                "Contact not found.");

        if (contact.UserId != userId)
            throw new UnauthorizedAccessException(
                "You do not have access to this contact.");

        var workspace =
            await _workspaceRepository
                .GetByIdAsync(workspaceId);

        if (workspace == null)
            throw new Exception(
                "Workspace not found.");

        if (workspace.UserId != userId)
            throw new UnauthorizedAccessException(
                "You do not have access to this workspace.");
    }

    // =========================================================
    // MAP
    // =========================================================

    private static ContactTrustDto MapToDto(
        ContactTrust trust)
    {
        return new ContactTrustDto
        {
            ContactId =
                trust.ContactId,

            WorkspaceId =
                trust.WorkspaceId,

            Score =
                trust.Score,

            Status =
                ContactTrustCalculator
                    .GetStatus(trust.Score)
                    .ToString(),

            UpdatedAt =
                trust.UpdatedAt
        };
    }
}