namespace SmartAccessioningPortal.Api.Services;

public interface IFileStorageService
{
    Task<StoredFile> SaveAsync(string folder, IFormFile file, CancellationToken cancellationToken = default);

    Task<StoredFileDownload?> OpenReadAsync(
        string storagePath,
        string contentType,
        CancellationToken cancellationToken = default);
}

public sealed record StoredFile(string StoragePath, string FileName, string ContentType);

public sealed record StoredFileDownload(Stream Stream, string ContentType);
