using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.FileUploads;

public class FileUploadResult
{
    public FileUpload FileUpload { get; set; }
    public string? Url { get; set; }
}