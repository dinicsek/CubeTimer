using System.ComponentModel.DataAnnotations;

namespace CubeTimer.WebApi.Data.FileUploads;

public class FileUploadRequestBody
{
    [Required]
    public IFormFile File { get; set; }
    
    [Required]
    public string Purpose { get; set; }
}