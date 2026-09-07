using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ILoanRepository
{
    // =========================================================
    // CREATE
    // =========================================================

    Task AddAsync(
        Loan loan);

    // =========================================================
    // GET LOAN BY ID
    // =========================================================

    Task<Loan?> GetByIdAsync(
        Guid id);

    // =========================================================
    // GET USER'S LOANS
    // =========================================================

    Task<List<Loan>> GetByUserAsync(
        Guid userId);

    // =========================================================
    // GET USER'S LOANS BY WORKSPACE
    // =========================================================

    Task<List<Loan>> GetByUserAndWorkspaceAsync(
        Guid userId,
        Guid workspaceId);

    // =========================================================
    // GET LOANS BY CONTACT
    // =========================================================

    Task<List<Loan>> GetByContactIdAsync(
        Guid contactId);

    // =========================================================
    // REPLACE LOAN ITEMS
    // =========================================================

    Task ReplaceItemsAsync(
        Guid loanId,
        List<LoanItem> newItems);

    // =========================================================
    // SAVE CHANGES
    // =========================================================

    Task SaveChangesAsync();
}