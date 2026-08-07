namespace LoanLedger.Domain.Entities;

public class LoanCategory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = "#4CAF50";

    public string Icon { get; set; } = "category";

    public bool IsDefault { get; set; }

    public bool IsArchived { get; set; }

    public User User { get; set; } = null!;
}