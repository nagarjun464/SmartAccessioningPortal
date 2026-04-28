namespace SmartAccessioningPortal.Web.Models;

public class CaseListItemModel
{
    public int CaseId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TestType { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}