using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace LoanLedger.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileStorageService(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    // =========================================================
	// SAVE FILE
	// =========================================================

	public async Task<string> SaveAsync(
		Stream fileStream,
		string fileName,
		string contentType,
		string folder = "attachments")
	{
		if (fileStream == null)
			throw new ArgumentNullException(nameof(fileStream));

		if (string.IsNullOrWhiteSpace(fileName))
			throw new ArgumentException(
				"File name is required.",
				nameof(fileName));

		if (string.IsNullOrWhiteSpace(folder))
			throw new ArgumentException(
				"Storage folder is required.",
				nameof(folder));

		// wwwroot/uploads/{folder}
		var storageFolder = Path.Combine(
			GetUploadsFolder(),
			folder);

		Directory.CreateDirectory(
			storageFolder);

		var extension =
			Path.GetExtension(fileName);

		var storedFileName =
			$"{Guid.NewGuid():N}{extension}";

		var physicalPath =
			Path.Combine(
				storageFolder,
				storedFileName);

		await using var outputStream =
			new FileStream(
				physicalPath,
				FileMode.CreateNew,
				FileAccess.Write,
				FileShare.None);

		await fileStream.CopyToAsync(
			outputStream);

		return $"{folder}/{storedFileName}";
	}

    // =========================================================
    // GET FILE
    // =========================================================

    public Task<Stream?> GetAsync(
        string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            return Task.FromResult<Stream?>(null);

        var physicalPath =
            GetPhysicalPath(storageKey);

        if (physicalPath == null ||
            !File.Exists(physicalPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        Stream stream =
            new FileStream(
                physicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

        return Task.FromResult<Stream?>(stream);
    }

    // =========================================================
    // DELETE FILE
    // =========================================================

    public Task<bool> DeleteAsync(
        string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            return Task.FromResult(false);

        var physicalPath =
            GetPhysicalPath(storageKey);

        if (physicalPath == null ||
            !File.Exists(physicalPath))
        {
            return Task.FromResult(false);
        }

        File.Delete(physicalPath);

        return Task.FromResult(true);
    }

    // =========================================================
    // UPLOADS FOLDER
    // =========================================================

    private string GetUploadsFolder()
    {
        var webRoot =
            _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot =
                Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
        }

        return Path.Combine(
            webRoot,
            "uploads");
    }

    // =========================================================
    // SAFE PHYSICAL PATH
    // =========================================================

    private string? GetPhysicalPath(
        string storageKey)
    {
        var uploadsRoot =
            Path.GetFullPath(
                GetUploadsFolder());

        var relativePath =
            storageKey
                .Replace(
                    '/',
                    Path.DirectorySeparatorChar)
                .TrimStart(
                    Path.DirectorySeparatorChar);

        var physicalPath =
            Path.GetFullPath(
                Path.Combine(
                    uploadsRoot,
                    relativePath));

        // Prevent path traversal such as:
        // ../../some-secret-file
        if (!physicalPath.StartsWith(
                uploadsRoot +
                Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return physicalPath;
    }
}