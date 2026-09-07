using LoanLedger.Application.Attachments;

namespace LoanLedger.Application.Interfaces;

public interface IAttachmentService
{
    Task<AttachmentResponse> UploadAsync(
        Guid userId,
        Guid loanId,
        Stream fileStream,
        string originalFileName,
        string contentType,
        string attachmentType,
        string? description);

    Task<List<AttachmentResponse>> GetByUserAsync(
        Guid userId);

    Task<List<AttachmentResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId);

    Task<AttachmentResponse?> GetByIdAsync(
        Guid userId,
        Guid id);

    Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(
        Guid userId,
        Guid id);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateAttachmentRequest request);

    Task<bool> DeleteAsync(
        Guid userId,
        Guid id);
}