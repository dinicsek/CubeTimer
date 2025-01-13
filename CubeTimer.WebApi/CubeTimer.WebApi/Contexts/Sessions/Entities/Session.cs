using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Infrastructure.Database.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CubeTimer.WebApi.Contexts.Sessions.Entities;

public class Session : TimestampedEntity
{
    public int Id { get; set; }
    
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
    
    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [JsonIgnore] public List<Solve> Solves { get; set; }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.SessionName);
        builder.HasIndex(s => s.Description);

        builder.Property(s => s.UserId).IsRequired();
        
        builder.HasOne(s => s.User).WithMany(u => u.Sessions).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.Solves).WithOne(s => s.Session);

    }
}