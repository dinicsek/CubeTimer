using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities.Enums;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Infrastructure.Database.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CubeTimer.WebApi.Contexts.Cubes.Entities;

public class Cube : TimestampedEntity
{
    public int Id { get; set; }

    public CubeEvent CubeEvent { get; set; }

    public string CubeMake { get; set; }

    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [JsonIgnore] public List<Solve> Solves { get; set; }
}

public class CubeConfiguration : IEntityTypeConfiguration<Cube>
{
    public void Configure(EntityTypeBuilder<Cube> builder)
    {
        builder.ToTable("Cubes");

        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.CubeEvent);
        builder.HasIndex(c=> c.CubeMake);

        builder.Property(c => c.CubeEvent).IsRequired();
        builder.Property(c => c.CubeMake).IsRequired();
        builder.Property(c => c.UserId).IsRequired();
        
        builder.HasOne(c => c.User).WithMany(u => u.Cubes).HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Solves).WithOne(s => s.Cube);
    }
}