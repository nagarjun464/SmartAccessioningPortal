namespace SmartAccessioningPortal.Domain.Entities;

public class TubePhoto
{
    public int TubePhotoId { get; set; }
    public int CaseId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
}