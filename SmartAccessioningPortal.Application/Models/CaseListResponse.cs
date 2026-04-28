namespace SmartAccessioningPortal.Application.Models;

public class CaseListResponse
{
    public int CaseId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TestType { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public string? PatientFirstName { get; set; }
    public string? PatientLastName { get; set; }
    public string? KitBoxCode { get; set; }
    public string? LotCode { get; set; }
}