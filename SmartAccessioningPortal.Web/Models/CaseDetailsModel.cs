namespace SmartAccessioningPortal.Web.Models;

public class CaseDetailsModel
{
    public int CaseId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TestType { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public PatientDetailsModel? Patient { get; set; }
    public KitInfoDetailsModel? KitInfo { get; set; }
    public List<DocumentDetailsModel> Documents { get; set; } = new();
}

public class PatientDetailsModel
{
    public int PatientId { get; set; }
    public int CaseId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DOB { get; set; }
    public string? MRN { get; set; }
}

public class KitInfoDetailsModel
{
    public int KitInfoId { get; set; }
    public int CaseId { get; set; }
    public string KitBoxCode { get; set; } = string.Empty;
    public string LotCode { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}

public class DocumentDetailsModel
{
    public int DocumentId { get; set; }
    public int CaseId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}