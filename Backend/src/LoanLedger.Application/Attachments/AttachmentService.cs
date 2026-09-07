using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Attachments;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentRepository _repository;
	private readonly ILoanRepository _loanRepository;
	private readonly IFileStorageService _fileStorage;

	public AttachmentService(
		IAttachmentRepository repository,
		ILoanRepository loanRepository,
		IFileStorageService fileStorage)
	{
		_repository = repository;
		_loanRepository = loanRepository;
		_fileStorage = fileStorage;
	}

    // =========================================================
    // UPLOAD
    // =========================================================

    public async Task<AttachmentResponse> UploadAsync(
        Guid userId,
        Guid loanId,
        Stream fileStream,
        string originalFileName,
        string contentType,
        string attachmentType,
        string? description)
    {
        if (loanId == Guid.Empty)
            throw new Exception("Loan ID is required.");

        if (fileStream == null)
            throw new Exception("File is required.");

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new Exception("File name is required.");

        if (string.IsNullOrWhiteSpace(contentType))
            throw new Exception("Content type is required.");

        if (string.IsNullOrWhiteSpace(attachmentType))
            throw new Exception("Attachment type is required.");

        var loan =
            await _loanRepository.GetByIdAsync(loanId);

        if (loan == null || loan.UserId != userId)
            throw new Exception("Loan not found.");

        var extension =
            Path.GetExtension(originalFileName);

        if (string.IsNullOrWhiteSpace(extension))
            throw new Exception("File extension is required.");

        var storageKey =
            await _fileStorage.SaveAsync(
                fileStream,
                originalFileName,
                contentType);

        var storedFileName =
            Path.GetFileName(storageKey);

        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            LoanId = loanId,

            OriginalFileName =
                originalFileName.Trim(),

            StoredFileName =
                storedFileName,

            FileExtension =
                extension.Trim(),

            MimeType =
                contentType.Trim(),

            FileSize =
                fileStream.CanSeek
                    ? fileStream.Length
                    : 0,

            AttachmentType =
                attachmentType.Trim(),

            StorageProvider =
                "Local",

            StorageKey =
                storageKey,

            Description =
                description?.Trim(),

            CreatedAt =
                DateTime.UtcNow
        };

        await _repository.AddAsync(attachment);

        await _repository.SaveChangesAsync();

        return Map(attachment);
    }

    // =========================================================
    // GET ALL BY USER
    // =========================================================

    public async Task<List<AttachmentResponse>> GetByUserAsync(
        Guid userId)
    {
        var attachments =
            await _repository.GetByUserAsync(userId);

        return attachments
            .Select(Map)
            .ToList();
    }

    // =========================================================
    // GET BY LOAN
    // =========================================================

    public async Task<List<AttachmentResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId)
    {
        var loan =
            await _loanRepository.GetByIdAsync(loanId);

        if (loan == null || loan.UserId != userId)
            throw new Exception("Loan not found.");

        var attachments =
            await _repository.GetByLoanAsync(
                userId,
                loanId);

        return attachments
            .Select(Map)
            .ToList();
    }

    // =========================================================
    // GET ONE
    // =========================================================

    public async Task<AttachmentResponse?> GetByIdAsync(
        Guid userId,
        Guid id)
    {
        var attachment =
            await _repository.GetByIdAsync(id);

        if (attachment == null ||
            attachment.UserId != userId)
        {
            return null;
        }

        return Map(attachment);
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateAttachmentRequest request)
    {
        var attachment =
            await _repository.GetByIdAsync(id);

        if (attachment == null ||
            attachment.UserId != userId)
        {
            return false;
        }

        attachment.Description =
            request.Description?.Trim();

        attachment.UpdatedAt =
            DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return true;
    }

	// =========================================================
	// DOWNLOAD FILE
	// =========================================================

	public async Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(
		Guid userId,
		Guid id)
	{
		var attachment =
			await _repository.GetByIdAsync(id);

		if (attachment == null ||
			attachment.UserId != userId)
		{
			return null;
		}

		var stream =
			await _fileStorage.GetAsync(
				attachment.StorageKey);

		if (stream == null)
		{
			return null;
		}

		return (
			stream,
			attachment.OriginalFileName,
			attachment.MimeType
		);
	}


    // =========================================================
    // DELETE
    // =========================================================

    public async Task<bool> DeleteAsync(
        Guid userId,
        Guid id)
    {
        var attachment =
            await _repository.GetByIdAsync(id);

        if (attachment == null ||
            attachment.UserId != userId)
        {
            return false;
        }

        await _fileStorage.DeleteAsync(
            attachment.StorageKey);

        await _repository.DeleteAsync(
            attachment);

        await _repository.SaveChangesAsync();

        return true;
    }

    // =========================================================
    // MAPPER
    // =========================================================

    private static AttachmentResponse Map(
        Attachment attachment)
    {
        return new AttachmentResponse
        {
            Id =
                attachment.Id,

            UserId =
                attachment.UserId,

            LoanId =
                attachment.LoanId,

            OriginalFileName =
                attachment.OriginalFileName,

            StoredFileName =
                attachment.StoredFileName,

            FileExtension =
                attachment.FileExtension,

            MimeType =
                attachment.MimeType,

            FileSize =
                attachment.FileSize,

            AttachmentType =
                attachment.AttachmentType,

            StorageProvider =
                attachment.StorageProvider,

            StorageKey =
                attachment.StorageKey,

            Description =
                attachment.Description,

            CreatedAt =
                attachment.CreatedAt,

            UpdatedAt =
                attachment.UpdatedAt
        };
    }
}