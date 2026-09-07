namespace LoanLedger.Application.Loans;

public class LoanResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

	public Guid? WorkspaceId { get; set; }
	
    public Guid ContactId { get; set; }

    public string ContactName { get; set; } = string.Empty;

    public Guid LoanCategoryId { get; set; }

    public string LoanCategory { get; set; } = string.Empty;
	
	public int Direction { get; set; }

	public string DirectionName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    public bool IsClosed { get; set; }

    public DateTime? ClosedAt { get; set; }
	public List<LoanItemDto> Items { get; set; }
    = new List<LoanItemDto>();
}