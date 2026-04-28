using System.ComponentModel.DataAnnotations;

namespace SmartAccessioningPortal.Web.Models;

public class KitInfoFormModel
{
    [Required]
    public string KitBoxCode { get; set; } = string.Empty;
    [Required]
    public string LotCode { get; set; } = string.Empty;
    [Required]
    public string OperatorName { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
}