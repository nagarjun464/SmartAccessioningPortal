namespace SmartAccessioningPortal.Web.Models;

using System.ComponentModel.DataAnnotations;

public class PatientFormModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public DateTime? DOB { get; set; }
    public string? MRN { get; set; }
}