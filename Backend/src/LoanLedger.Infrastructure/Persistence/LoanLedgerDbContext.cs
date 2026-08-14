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
	public DbSet<Loan> Loans => Set<Loan>();
	public DbSet<LoanTransaction> LoanTransactions   => Set<LoanTransaction>();

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
		modelBuilder.Entity<Loan>()
			.HasOne(l => l.User)
			.WithMany(u => u.Loans)
			.HasForeignKey(l => l.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Loan>()
			.HasOne(l => l.Contact)
			.WithMany(c => c.Loans)
			.HasForeignKey(l => l.ContactId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Loan>()
			.HasOne(l => l.LoanCategory)
			.WithMany(c => c.Loans)
			.HasForeignKey(l => l.LoanCategoryId)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<LoanTransaction>()
			.HasOne(t => t.Loan)
			.WithMany(l => l.Transactions)
			.HasForeignKey(t => t.LoanId)
			.OnDelete(DeleteBehavior.Cascade);
    }
}