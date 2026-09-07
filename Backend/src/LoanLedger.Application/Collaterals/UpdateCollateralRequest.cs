namespace LoanLedger.Application.Collaterals;

public class UpdateCollateralRequest
{
    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal? EstimatedValue { get; set; }

    public string? Location { get; set; }

    public string? Ownership { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }
}