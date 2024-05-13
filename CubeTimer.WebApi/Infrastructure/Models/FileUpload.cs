using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CubeTimer.WebApi.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubeTimer.WebApi.Infrastructure.Models;

public class FileUpload : TimestampedEntity
{
    [Key]
    public int Id { get; set; }
    
    public string Filename { get; set; }
    
    public string OriginalFilename { get; set; }
    
    public string MimeType { get; set; }
    
    public string Path { get; set; }
    
    public string Purpose { get; set; }
    
    [JsonIgnore] public User User { get; set; }
    
    public int UserId { get; set; }
}

public class FileUploadConfiguration : IEntityTypeConfiguration<FileUpload>
{
    public void Configure(EntityTypeBuilder<FileUpload> builder)
    {
        builder.HasOne(i => i.User).WithMany(u => u.FileUploads).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

