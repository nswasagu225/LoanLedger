namespace LoanLedger.Application.Attachments;

public class AttachmentResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid LoanId { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string AttachmentType { get; set; } = string.Empty;

    public string StorageProvider { get; set; } = string.Empty;

    public string StorageKey { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}