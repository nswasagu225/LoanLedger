namespace LoanLedger.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid LoanId { get; set; }

    public Loan Loan { get; set; } = null!;

    // =========================================================
    // FILE INFORMATION
    // =========================================================

    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    // =========================================================
    // ATTACHMENT TYPE
    // =========================================================

    public string AttachmentType { get; set; } = string.Empty;

    // =========================================================
    // STORAGE
    // =========================================================

    public string StorageProvider { get; set; } = string.Empty;

    public string StorageKey { get; set; } = string.Empty;

    // =========================================================
    // OPTIONAL INFORMATION
    // =========================================================

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}