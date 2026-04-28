namespace SmartAccessioningPortal.Web.Models;
using System.ComponentModel.DataAnnotations;

public class CreateCaseFormModel
{
    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    [Required]
    public string? TestType { get; set; }
}