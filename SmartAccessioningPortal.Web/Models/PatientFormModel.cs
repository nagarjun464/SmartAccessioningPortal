namespace SmartAccessioningPortal.Web.Models;

public class PatientFormModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DOB { get; set; }
    public string? MRN { get; set; }
}