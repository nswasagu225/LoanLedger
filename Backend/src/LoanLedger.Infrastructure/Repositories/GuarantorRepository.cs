using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class GuarantorRepository : IGuarantorRepository
{
    private readonly LoanLedgerDbContext _context;

    public GuarantorRepository(
        LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Guarantor guarantor)
    {
        await _context.Guarantors.AddAsync(guarantor);
    }

    public async Task<Guarantor?> GetByIdAsync(
        Guid userId,
        Guid guarantorId)
    {
        return await _context.Guarantors
            .FirstOrDefaultAsync(x =>
                x.Id == guarantorId &&
                x.UserId == userId &&
                !x.IsDeleted);
    }

    public async Task<List<Guarantor>> GetByUserAsync(
        Guid userId)
    {
        return await _context.Guarantors
            .Where(x =>
                x.UserId == userId &&
                !x.IsDeleted)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<Guarantor?> GetByPhoneAsync(
        Guid userId,
        string phoneNumber)
    {
        return await _context.Guarantors
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.PhoneNumber == phoneNumber &&
                !x.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}