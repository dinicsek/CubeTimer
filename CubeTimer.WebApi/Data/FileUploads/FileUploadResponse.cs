namespace CubeTimer.WebApi.Data.FileUploads;

public class FileUploadResponse
{
    public int Id { get; set; }
    
    public string FileName { get; set; }
    
    public string OriginalFileName { get; set; }
    
    public string Path { get; set; }
    
    public string Purpose { get; set; }
    
    public string? Url { get; set; }
    
    public string MimeType { get; set; }
}