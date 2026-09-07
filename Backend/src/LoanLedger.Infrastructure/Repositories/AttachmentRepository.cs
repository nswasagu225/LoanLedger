using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly LoanLedgerDbContext _context;

    public AttachmentRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    // =========================================================
    // ADD
    // =========================================================

    public async Task AddAsync(Attachment attachment)
    {
        await _context.Attachments.AddAsync(attachment);
    }

    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<Attachment?> GetByIdAsync(Guid id)
    {
        return await _context.Attachments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // =========================================================
    // GET ALL BY USER
    // =========================================================

    public async Task<List<Attachment>> GetByUserAsync(
        Guid userId)
    {
        return await _context.Attachments
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // =========================================================
    // GET BY LOAN
    // =========================================================

    public async Task<List<Attachment>> GetByLoanAsync(
        Guid userId,
        Guid loanId)
    {
        return await _context.Attachments
            .Where(x =>
                x.UserId == userId &&
                x.LoanId == loanId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // =========================================================
    // DELETE
    // =========================================================

    public async Task DeleteAsync(Attachment attachment)
    {
        _context.Attachments.Remove(attachment);

        await Task.CompletedTask;
    }

    // =========================================================
    // SAVE CHANGES
    // =========================================================

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}