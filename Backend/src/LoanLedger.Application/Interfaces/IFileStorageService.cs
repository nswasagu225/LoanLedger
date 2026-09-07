namespace LoanLedger.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string folder = "attachments");

    Task<Stream?> GetAsync(
        string storageKey);

    Task<bool> DeleteAsync(
        string storageKey);
}