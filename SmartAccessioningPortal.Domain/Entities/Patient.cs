namespace SmartAccessioningPortal.Domain.Entities;

public class Patient
{
    public int PatientId { get; set; }
    public int CaseId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DOB { get; set; }
    public string? MRN { get; set; }

    public Case? Case { get; set; }
}