namespace CubeTimer.WebApi.Data.FileUploads;

public class IndexFileUploadResponse
{
    public int Id { get; set; }
    
    public string OriginalFileName { get; set; }
    
    public string MimeType { get; set; }
    
    public string Purpose { get; set; }
    
    public string Url { get; set; }
}