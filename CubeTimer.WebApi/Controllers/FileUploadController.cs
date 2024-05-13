using CubeTimer.WebApi.Data.FileUploads;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class FileUploadController : ControllerBase
{
   private readonly ApplicationDbContext _context;
   private readonly FileService _fileService;
   
   public FileUploadController(ApplicationDbContext context, FileService fileService)
   {
       _context = context;
       _fileService = fileService;
   }

   [HttpGet]
   [EnableCors]
   [Authorize]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<ActionResult<IEnumerable<IndexFileUploadResponse>>> Index()
   {
       var fileUploads = await _context.FileUploads.ToListAsync();

       return Ok(fileUploads.Select(f => new IndexFileUploadResponse
       {
           Id = f.Id,
           Purpose = f.Purpose,
           Url = _fileService.GetUrl(f.Path)!
       }));
   }

   [HttpGet("{id}")]
   [EnableCors]
   [Authorize]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<ActionResult<ViewFileUploadResponse>> View([FromRoute] int id)
   {
       var fileUpload = await _context.FileUploads.FindAsync(id);

       if (fileUpload == null)
       {
           return NotFound("The file upload was not found.");
       }

       return Ok(new ViewFileUploadResponse
       {
           Id = fileUpload.Id,
           Purpose = fileUpload.Purpose,
           Url = _fileService.GetUrl(fileUpload.Path)!
       });
   }
   
   [HttpPost]
   [Authorize]
   [EnableCors]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<FileUploadResponse>> Create([FromForm] FileUploadRequestBody body)
   {
       var acceptedMimeTypes = new HashSet<string> { "image/jpeg", "image/png", "image/gif" }; 
       if (!acceptedMimeTypes.Contains(body.File.ContentType))
       { 
           return BadRequest("The file must be a valid image file.");
       }
       
       var temp = await _fileService.UploadFileAsync(body.File);

       await _context.SaveChangesAsync();

       return Ok(new FileUploadResponse
       {
           Id = temp.FileUpload.Id,
           FileName = temp.FileUpload.Filename,
           OriginalFileName = temp.FileUpload.OriginalFilename,
           Path = temp.FileUpload.Path,
           Purpose = body.Purpose,
           Url = temp.Url,
           MimeType = temp.FileUpload.MimeType
       });
   }
   
   [HttpDelete("{id}")]
   [EnableCors]
   [Authorize]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<ActionResult> Delete([FromRoute] int id)
   {
       var fileUpload = await _context.FileUploads.FindAsync(id);
       if (fileUpload == null)
       {
           return NotFound();
       }
       _context.FileUploads.Remove(fileUpload);
       await _context.SaveChangesAsync();
       _fileService.DeleteFile(fileUpload);
       return NoContent();
   }
}