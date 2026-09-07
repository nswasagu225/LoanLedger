namespace LoanLedger.Application.Attachments;

// =========================================================
// CREATE ATTACHMENT REQUEST
// =========================================================

public class CreateAttachmentRequest
{
    public Guid LoanId { get; set; }

    public string AttachmentType { get; set; } = string.Empty;

    public string? Description { get; set; }
}


// =========================================================
// UPDATE ATTACHMENT REQUEST
// =========================================================

public class UpdateAttachmentRequest
{
    public string? Description { get; set; }
}