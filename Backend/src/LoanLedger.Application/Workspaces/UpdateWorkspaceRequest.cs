using System.ComponentModel.DataAnnotations;
using LoanLedger.Application.Common.Validation;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Workspaces;

public class UpdateWorkspaceRequest
{
    [Required(
        ErrorMessage = "Workspace name is required.")]
    [RequiredStringLength(
        2,
        150,
        ErrorMessage =
            "Workspace name must be between 2 and 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(
        ErrorMessage = "Workspace type is required.")]
    [EnumDataType(
        typeof(WorkspaceType),
        ErrorMessage = "Invalid workspace type.")]
    public WorkspaceType Type { get; set; }

    [StringLength(
        1000,
        ErrorMessage =
            "Workspace description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [RegularExpression(
        @"^[A-Za-z]{3}$",
        ErrorMessage =
            "Currency must be a valid 3-letter currency code, such as NGN, USD, or GBP.")]
    public string? Currency { get; set; }
}