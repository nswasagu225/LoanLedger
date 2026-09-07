using Microsoft.AspNetCore.Http;

namespace LoanLedger.API.Models;

public class UploadAttachmentRequest
{
    public Guid LoanId { get; set; }

    public IFormFile? File { get; set; }

    public string AttachmentType { get; set; } = string.Empty;

    public string? Description { get; set; }
}