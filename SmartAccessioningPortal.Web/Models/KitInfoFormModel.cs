namespace SmartAccessioningPortal.Web.Models;

public class KitInfoFormModel
{
    public string KitBoxCode { get; set; } = string.Empty;
    public string LotCode { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
}