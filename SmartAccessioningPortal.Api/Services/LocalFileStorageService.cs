namespace SmartAccessioningPortal.Api.Services;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _uploadsRoot;

    public LocalFileStorageService()
    {
        _uploadsRoot = Path.Combine(Path.GetTempPath(), "Uploads");
    }

    public async Task<StoredFile> SaveAsync(string folder, IFormFile file, CancellationToken cancellationToken = default)
    {
        var storagePath = FileStorageNames.CreateObjectName(folder, file.FileName);
        var fullFolder = Path.Combine(_uploadsRoot, folder);
        Directory.CreateDirectory(fullFolder);

        var fullPath = Path.Combine(_uploadsRoot, storagePath.Replace('/', Path.DirectorySeparatorChar));

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return new StoredFile(storagePath, file.FileName, file.ContentType);
    }

    public Task<StoredFileDownload?> OpenReadAsync(
        string storagePath,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var fullPath = Path.IsPathRooted(storagePath)
            ? storagePath
            : Path.Combine(_uploadsRoot, storagePath.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<StoredFileDownload?>(null);
        }

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<StoredFileDownload?>(new StoredFileDownload(stream, contentType));
    }
}
