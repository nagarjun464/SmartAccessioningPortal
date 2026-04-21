namespace SmartAccessioningPortal.Domain.Entities;

public class Case
{
    public int CaseId { get; set; }
    public string Status { get; set; } = "Draft";
    public string? TestType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
    public KitInfo? KitInfo { get; set; }
    public List<Document> Documents { get; set; } = new();
}