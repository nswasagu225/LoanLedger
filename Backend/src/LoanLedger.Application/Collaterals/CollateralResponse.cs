namespace LoanLedger.Application.Collaterals;

public class CollateralResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid LoanId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal? EstimatedValue { get; set; }

    public string? Location { get; set; }

    public string? Ownership { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }

    public bool IsReleased { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}