using Microsoft.EntityFrameworkCore;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Infrastructure.Persistence;

public class LoanLedgerDbContext : DbContext
{
    public LoanLedgerDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Contact> Contacts => Set<Contact>();
	public DbSet<LoanCategory> LoanCategories => Set<LoanCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<LoanCategory>()
			.HasOne(c => c.User)
			.WithMany(u => u.LoanCategories)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Cascade);
    }
}