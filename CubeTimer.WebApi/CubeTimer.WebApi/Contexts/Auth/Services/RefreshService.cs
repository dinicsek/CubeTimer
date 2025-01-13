using System.Security.Cryptography;
using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Services;

public class RefreshService(ApplicationDbContext dbContext) : IRefreshService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public bool TryCreateRefreshToken(int userId, DateTime expireAt, string? parentRefreshToken,
        out RefreshToken? refreshToken)
    {
        int? parentTokenId = null;
        if (parentRefreshToken != null)
        {
            var parentToken = _dbContext.Set<RefreshToken>().Include(t => t.ChildToken)
                .FirstOrDefault(t => t.Token == parentRefreshToken);
            parentTokenId = parentToken?.Id;

            if (parentToken?.ChildToken != null)
            {
                refreshToken = null;
                return false;
            }
        }

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = expireAt,
            ParentTokenId = parentTokenId
        };

        return true;
    }

    public bool ValidateRefreshToken(string token, out RefreshToken? refreshToken)
    {
        refreshToken = _dbContext.Set<RefreshToken>().FirstOrDefault(t => t.Token == token);
        return refreshToken != null && refreshToken.ExpiresAt.ToUniversalTime() > DateTime.UtcNow;
    }
}