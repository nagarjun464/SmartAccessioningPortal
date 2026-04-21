namespace SmartAccessioningPortal.Application.Models;

public class CreateCaseRequest
{
    public string CreatedBy { get; set; } = string.Empty;
    public string? TestType { get; set; }
}