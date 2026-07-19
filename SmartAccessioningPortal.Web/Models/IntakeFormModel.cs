using System.ComponentModel.DataAnnotations;

namespace SmartAccessioningPortal.Web.Models;

public class IntakeFormModel
{
    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public string? TestType { get; set; }

    public string? RelabelCode { get; set; }

    public bool NoKitCode { get; set; }

    public bool NoRelabelCode { get; set; }

    public string? ShipmentCode { get; set; }

    public string? AccessioningCase { get; set; }

    public string? SampleCode { get; set; }

    public bool IdentifiersConfirmed { get; set; }

    public bool IdentifiersMissing { get; set; }

    public string? RackCode { get; set; }

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
