using Google.Cloud.Storage.V1;

namespace SmartAccessioningPortal.Api.Services;

public sealed class GcpStorageService : IFileStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GcpStorageService(IConfiguration configuration)
    {
        _storageClient = StorageClient.Create();
        _bucketName = configuration["GcpStorage:BucketName"]
            ?? throw new InvalidOperationException("GcpStorage:BucketName is required when using Google Cloud Storage.");
    }

    public async Task<StoredFile> SaveAsync(string folder, IFormFile file, CancellationToken cancellationToken = default)
    {
        var objectName = FileStorageNames.CreateObjectName(folder, file.FileName);

        await using var stream = file.OpenReadStream();
        await _storageClient.UploadObjectAsync(
            _bucketName,
            objectName,
            file.ContentType,
            stream,
            cancellationToken: cancellationToken);

        return new StoredFile(objectName, file.FileName, file.ContentType);
    }

    public async Task<StoredFileDownload?> OpenReadAsync(
        string storagePath,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();

        try
        {
            await _storageClient.DownloadObjectAsync(
                _bucketName,
                storagePath,
                stream,
                cancellationToken: cancellationToken);
        }
        catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
        {
            await stream.DisposeAsync();
            return null;
        }

        stream.Position = 0;
        return new StoredFileDownload(stream, contentType);
    }
}
