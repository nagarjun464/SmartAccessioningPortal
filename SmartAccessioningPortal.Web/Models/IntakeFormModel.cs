using System.ComponentModel.DataAnnotations;

namespace SmartAccessioningPortal.Web.Models;

public class IntakeFormModel
{
    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public string? TestType { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public DateTime? DOB { get; set; }

    public string? MRN { get; set; }

    [Required]
    public string KitBoxCode { get; set; } = string.Empty;

    [Required]
    public string LotCode { get; set; } = string.Empty;

    [Required]
    public string OperatorName { get; set; } = string.Empty;

    public DateTime? ReceivedAt { get; set; }
}