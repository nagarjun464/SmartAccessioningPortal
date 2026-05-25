namespace SmartAccessioningPortal.Web.Models;

public class TubePhotoModel
{
    public int TubePhotoId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; }
}