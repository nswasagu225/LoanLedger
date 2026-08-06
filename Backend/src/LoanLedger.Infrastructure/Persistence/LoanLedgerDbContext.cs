using Microsoft.EntityFrameworkCore;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Infrastructure.Persistence;

public class LoanLedgerDbContext : DbContext
{
    public LoanLedgerDbContext(DbContextOptions<LoanLedgerDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}