using System.Text.Json.Serialization;

namespace SmartAccessioningPortal.Domain.Entities;

public class Document
{
    public int DocumentId { get; set; }
    public int CaseId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public Case? Case { get; set; }
}