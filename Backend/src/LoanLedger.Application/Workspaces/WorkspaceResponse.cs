using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Workspaces;

public class WorkspaceResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public WorkspaceType Type { get; set; }

    public string TypeName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Currency { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}