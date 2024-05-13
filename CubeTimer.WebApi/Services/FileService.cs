using CubeTimer.WebApi.Data.FileUploads;
using CubeTimer.WebApi.Infrastructure;
using FileUpload = CubeTimer.WebApi.Infrastructure.Models.FileUpload;

namespace CubeTimer.WebApi.Services;

public class FileService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    
    public FileService(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task<FileUploadResult> UploadFileAsync(IFormFile file)
    {
        var uniqueFileName = GetUniqueFileName(file.FileName);
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FileUploads");
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

        var fileUpload = new FileUpload
        {
            Filename = uniqueFileName,
            OriginalFilename = file.FileName,
            Path = filePath,
            MimeType = file.ContentType,
        };

        await _context.FileUploads.AddAsync(fileUpload);

        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            return new FileUploadResult
            {
                FileUpload = fileUpload
            };

        var request = httpContextAccessor.HttpContext.Request;

        return new FileUploadResult
        {
            FileUpload = fileUpload,
            Url = GetUrl(fileUpload.Path)
        };
    }
    public void DeleteFile(FileUpload fileUpload)
    {
        File.Delete(fileUpload.Path);

        _context.FileUploads.Remove(fileUpload);
    }

    public string? GetUrl(string path)
    {
        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            return null;

        var request = httpContextAccessor.HttpContext.Request;

        return $"{request.Scheme}://{request.Host}{request.PathBase}/FileUploads/{Path.GetFileName(path)}";
    }
    private string GetUniqueFileName(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        return string.Concat(Path.GetFileNameWithoutExtension(fileName)
            , "_"
            , Guid.NewGuid().ToString().AsSpan(0, 8)
            , Path.GetExtension(fileName));
    }
}