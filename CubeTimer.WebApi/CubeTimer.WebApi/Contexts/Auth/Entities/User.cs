using CubeTimer.WebApi.Infrastructure.Database.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CubeTimer.WebApi.Contexts.Auth.Entities;

public class User : TimestampedEntity
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string PasswordHashed { get; set; }
    
    public Role Role { get; set; }
    
    [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; }

    [JsonIgnore] public List<Session> Sessions { get; set; }

    [JsonIgnore] public List<Cube> Cubes { get; set; }

    [JsonIgnore] public List<Solve> Solves { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.PasswordHashed).IsRequired();
        builder.Property(u => u.Role).IsRequired().HasConversion(new EnumToStringConverter<Role>());
        
        builder.HasMany(u => u.Cubes).WithOne(c => c.User);
        builder.HasMany(u => u.RefreshTokens).WithOne(t => t.User);
        builder.HasMany(u => u.Sessions).WithOne(s => s.User);
        builder.HasMany(u => u.Solves).WithOne(s => s.User);
    }
}