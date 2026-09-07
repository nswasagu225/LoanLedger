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
	public DbSet<Workspace> Workspaces { get; set; }
	public DbSet<Guarantor> Guarantors => Set<Guarantor>();
	public DbSet<Witness> Witnesses => Set<Witness>();
	public DbSet<LoanWitness> LoanWitnesses => Set<LoanWitness>();
	public DbSet<Item> Items => Set<Item>();
	public DbSet<LoanItem> LoanItems => Set<LoanItem>();
	public DbSet<ContactTrust> ContactTrusts => Set<ContactTrust>();
	public DbSet<Collateral> Collaterals => Set<Collateral>();
	public DbSet<Attachment> Attachments => Set<Attachment>();

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
		modelBuilder.Entity<Loan>()
			.HasOne(l => l.Workspace)
			.WithMany(w => w.Loans)
			.HasForeignKey(l => l.WorkspaceId)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<LoanTransaction>()
			.HasOne(t => t.Loan)
			.WithMany(l => l.Transactions)
			.HasForeignKey(t => t.LoanId)
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<Workspace>(entity =>
		{
			entity.ToTable("Workspaces");

			entity.HasKey(w => w.Id);

			entity.Property(w => w.Name)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(w => w.Description)
				.HasMaxLength(500);

			entity.Property(w => w.Currency)
				.HasMaxLength(10);

			entity.Property(w => w.Type)
				.IsRequired();

			entity.Property(w => w.IsActive)
				.IsRequired();

			entity.HasOne(w => w.User)
				.WithMany(u => u.Workspaces)
				.HasForeignKey(w => w.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasIndex(w => w.UserId);
		});
		modelBuilder.Entity<Guarantor>()
			.HasOne(g => g.User)
			.WithMany(u => u.Guarantors)
			.HasForeignKey(g => g.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<Loan>()
			.HasOne(l => l.Guarantor)
			.WithMany(g => g.Loans)
			.HasForeignKey(l => l.GuarantorId)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<Guarantor>(entity =>
		{
			entity.ToTable("Guarantors");

			entity.HasKey(g => g.Id);

			entity.Property(g => g.FullName)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(g => g.PhoneNumber)
				.IsRequired()
				.HasMaxLength(30);

			entity.Property(g => g.Email)
				.HasMaxLength(150);

			entity.Property(g => g.Address)
				.HasMaxLength(500);

			entity.Property(g => g.Relationship)
				.HasMaxLength(100);

			entity.Property(g => g.IdentificationType)
				.HasMaxLength(50);

			entity.Property(g => g.IdentificationNumber)
				.HasMaxLength(100);

			entity.Property(g => g.Notes)
				.HasMaxLength(500);

			entity.HasIndex(g => g.UserId);
		});
		modelBuilder.Entity<Witness>()
			.HasOne(w => w.User)
			.WithMany(u => u.Witnesses)
			.HasForeignKey(w => w.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<LoanWitness>()
			.HasOne(lw => lw.Loan)
			.WithMany(l => l.LoanWitnesses)
			.HasForeignKey(lw => lw.LoanId)
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<LoanWitness>()
			.HasOne(lw => lw.Witness)
			.WithMany(w => w.LoanWitnesses)
			.HasForeignKey(lw => lw.WitnessId)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<Witness>(entity =>
		{
			entity.ToTable("Witnesses");

			entity.HasKey(w => w.Id);

			entity.Property(w => w.FullName)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(w => w.PhoneNumber)
				.IsRequired()
				.HasMaxLength(30);

			entity.Property(w => w.Address)
				.HasMaxLength(500);

			entity.Property(w => w.Relationship)
				.HasMaxLength(100);

			entity.Property(w => w.Notes)
				.HasMaxLength(500);

			entity.HasIndex(w => w.UserId);
		});
		modelBuilder.Entity<LoanWitness>(entity =>
		{
			entity.ToTable("LoanWitnesses");

			entity.HasKey(lw => lw.Id);

			entity.Property(lw => lw.WitnessOrder)
				.IsRequired();

			entity.HasIndex(lw => new
			{
				lw.LoanId,
				lw.WitnessOrder
			})
			.IsUnique();

			entity.HasIndex(lw => new
			{
				lw.LoanId,
				lw.WitnessId
			})
			.IsUnique();
		});
		// =========================================================
		// ITEM
		// =========================================================

		modelBuilder.Entity<Item>(entity =>
		{
			entity.ToTable("Items");

			entity.HasKey(i => i.Id);

			entity.Property(i => i.Name)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(i => i.Description)
				.HasMaxLength(500);

			entity.Property(i => i.Unit)
				.HasMaxLength(50);

			entity.Property(i => i.DefaultRate)
				.HasPrecision(18, 2);

			entity.HasOne(i => i.User)
				.WithMany(u => u.Items)
				.HasForeignKey(i => i.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasIndex(i => i.UserId);
		});
		// =========================================================
		// LOAN ITEMS
		// =========================================================

		modelBuilder.Entity<LoanItem>(entity =>
		{
			entity.ToTable("LoanItems");

			entity.HasKey(li => li.Id);

			entity.Property(li => li.ItemName)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(li => li.Unit)
				.HasMaxLength(50);

			entity.Property(li => li.Quantity)
				.HasPrecision(18, 4);

			entity.Property(li => li.Rate)
				.HasPrecision(18, 2);

			entity.Property(li => li.Amount)
				.HasPrecision(18, 2);

			entity.Property(li => li.Notes)
				.HasMaxLength(500);

			entity.HasOne(li => li.Loan)
				.WithMany(l => l.LoanItems)
				.HasForeignKey(li => li.LoanId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(li => li.Item)
				.WithMany(i => i.LoanItems)
				.HasForeignKey(li => li.ItemId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasIndex(li => li.LoanId);

			entity.HasIndex(li => li.ItemId);
		});
		// =========================================================
		// CONTACT TRUST
		// =========================================================

		modelBuilder.Entity<ContactTrust>(entity =>
		{
			entity.ToTable("ContactTrusts");

			entity.HasKey(ct => ct.Id);

			entity.Property(ct => ct.Score)
				.HasPrecision(5, 2);

			entity.Property(ct => ct.UpdatedAt)
				.IsRequired();

			entity.HasOne(ct => ct.Contact)
				.WithMany(c => c.TrustProfiles)
				.HasForeignKey(ct => ct.ContactId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(ct => ct.Workspace)
				.WithMany(w => w.ContactTrusts)
				.HasForeignKey(ct => ct.WorkspaceId)
				.OnDelete(DeleteBehavior.Cascade);

			// A contact can have only ONE trust profile
			// inside a particular workspace.
			entity.HasIndex(ct => new
			{
				ct.ContactId,
				ct.WorkspaceId
			})
			.IsUnique();
		});
		modelBuilder.Entity<Collateral>()
			.HasOne(c => c.User)
			.WithMany(u => u.Collaterals)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Collateral>()
			.HasOne(c => c.Loan)
			.WithMany(l => l.Collaterals)
			.HasForeignKey(c => c.LoanId)
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<Collateral>(entity =>
		{
			entity.ToTable("Collaterals");

			entity.Property(c => c.Type)
				.IsRequired();

			entity.Property(c => c.Description)
				.IsRequired();

			entity.Property(c => c.EstimatedValue)
				.HasPrecision(18, 2);

			entity.Property(c => c.IsReleased)
				.HasDefaultValue(false);

			entity.HasIndex(c => c.UserId);

			entity.HasIndex(c => c.LoanId);
		});
		modelBuilder.Entity<Attachment>(entity =>
		{
			entity.ToTable("Attachments");

			entity.HasKey(a => a.Id);

			entity.Property(a => a.OriginalFileName)
				.IsRequired()
				.HasMaxLength(255);

			entity.Property(a => a.StoredFileName)
				.IsRequired()
				.HasMaxLength(255);

			entity.Property(a => a.FileExtension)
				.IsRequired()
				.HasMaxLength(20);

			entity.Property(a => a.MimeType)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(a => a.FileSize)
				.IsRequired();

			entity.Property(a => a.AttachmentType)
				.IsRequired()
				.HasMaxLength(50);

			entity.Property(a => a.StorageProvider)
				.IsRequired()
				.HasMaxLength(50);

			entity.Property(a => a.StorageKey)
				.IsRequired()
				.HasMaxLength(500);

			entity.Property(a => a.Description)
				.HasMaxLength(500);

			// =========================================================
			// USER RELATIONSHIP
			// =========================================================

			entity.HasOne(a => a.User)
				.WithMany(u => u.Attachments)
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			// =========================================================
			// LOAN RELATIONSHIP
			// =========================================================

			entity.HasOne(a => a.Loan)
				.WithMany(l => l.Attachments)
				.HasForeignKey(a => a.LoanId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasIndex(a => a.UserId);

			entity.HasIndex(a => a.LoanId);
		});
		
    }
}