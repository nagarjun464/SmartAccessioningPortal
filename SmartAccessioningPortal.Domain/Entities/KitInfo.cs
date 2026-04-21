namespace SmartAccessioningPortal.Domain.Entities;

public class KitInfo
{
    public int KitInfoId { get; set; }
    public int CaseId { get; set; }
    public string KitBoxCode { get; set; } = string.Empty;
    public string LotCode { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public Case? Case { get; set; }
}