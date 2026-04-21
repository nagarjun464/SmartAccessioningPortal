namespace SmartAccessioningPortal.Application.Models;

public class SavePatientRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DOB { get; set; }
    public string? MRN { get; set; }
}