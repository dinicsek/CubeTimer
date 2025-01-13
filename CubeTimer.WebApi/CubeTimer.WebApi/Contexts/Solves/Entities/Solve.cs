using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CubeTimer.WebApi.Contexts.Solves.Entities;

public class Solve : TimestampedEntity
{
    public int Id { get; set; }

    public int Time { get; set; }

    public SolveModifier? SolveModifier { get; set; }

    public string Scramble { get; set; }

    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    public int? SessionId { get; set; }
    [JsonIgnore] public Session Session { get; set; }

    public int? CubeId { get; set; }
    [JsonIgnore] public Cube Cube { get; set; }
}

public class SolveConfiguration : IEntityTypeConfiguration<Solve>
{
    public void Configure(EntityTypeBuilder<Solve> builder)
    {
        builder.ToTable("Solves");

        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Time);
        builder.HasIndex(s => s.SolveModifier);
        builder.HasIndex(s => s.Scramble);

        builder.HasOne(s => s.User).WithMany(u => u.Solves).HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Session).WithMany(s => s.Solves).HasForeignKey(s => s.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Cube).WithMany(c => c.Solves).HasForeignKey(s => s.CubeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}