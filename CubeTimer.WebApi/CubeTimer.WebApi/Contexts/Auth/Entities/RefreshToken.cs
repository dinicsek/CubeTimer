using CubeTimer.WebApi.Infrastructure.Database.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace CubeTimer.WebApi.Contexts.Auth.Entities;

public class RefreshToken : TimestampedEntity
{
    public int Id { get; set; }

    public string Token { get; set; }

    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    public int? ParentTokenId { get; set; }
    [JsonIgnore] public RefreshToken? ParentToken { get; set; }
    [JsonIgnore] public RefreshToken? ChildToken { get; set; }

    public DateTime ExpiresAt { get; set; }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.Token).IsUnique();

        builder.Property(t => t.Token).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.ExpiresAt).IsRequired();

        builder.HasOne(t => t.User).WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.ParentToken).WithOne(t => t.ChildToken)
            .HasForeignKey<RefreshToken>(t => t.ParentTokenId).OnDelete(DeleteBehavior.Cascade);
    }
}