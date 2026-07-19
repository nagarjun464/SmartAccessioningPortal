namespace SmartAccessioningPortal.Api.Services;

public static class FileStorageNames
{
    public static string CreateObjectName(string folder, string originalFileName)
    {
        var safeFolder = SanitizeSegment(folder);
        var safeFileName = SanitizeSegment(Path.GetFileName(originalFileName));

        return $"{safeFolder}/{Guid.NewGuid():N}_{safeFileName}";
    }

    private static string SanitizeSegment(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var cleaned = new string(value
            .Select(ch => invalidChars.Contains(ch) || char.IsWhiteSpace(ch) ? '-' : ch)
            .ToArray());

        return string.IsNullOrWhiteSpace(cleaned) ? "file" : cleaned;
    }
}
