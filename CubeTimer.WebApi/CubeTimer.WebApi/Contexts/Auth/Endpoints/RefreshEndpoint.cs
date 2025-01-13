using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Services;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FastEndpoints.Security;
using Mapster;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints;

internal sealed class RefreshEndpoint(ApplicationDbContext dbContext, IRefreshService refreshService) : Endpoint<RefreshRequest, RefreshResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IRefreshService _refreshService = refreshService;
    
    public override void Configure()
    {
        Post("auth/refresh");
        AllowAnonymous();
        Throttle(30, 60);
        
        Options(o =>
        {
            o.WithTags("Auth");
        });
        Description(o =>
        {
            o.ProducesProblemFE();
        });
    }

    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var token = req.RefreshToken ?? HttpContext.Request.Cookies["CubeTimerRefresh"];

        if (token == null)
        {
            ThrowError("No refresh token provided");
        }

        if (_refreshService.ValidateRefreshToken(token, out var refreshToken))
        {
            ThrowError("Invalid refresh token");
        }
        
        await _dbContext.Entry(refreshToken!).Reference(t => t.User).LoadAsync(ct);

        if (_refreshService.TryCreateRefreshToken(refreshToken!.UserId, DateTime.Now.AddMonths(1), token, out var newRefreshToken))
        {
            var newToken = JwtBearer.CreateToken(o =>
            {
                o.SigningKey = Config["Jwt:SigningKey"]!;
                o.Issuer = Config["Jwt:Issuer"];
                o.ExpireAt = DateTime.UtcNow.AddHours(1);

                o.User[ClaimTypes.NameIdentifier] = refreshToken.UserId.ToString();
                o.User[ClaimTypes.Name] = refreshToken.User.Username;
                
                o.User.Roles.Add(refreshToken.User.Role.ToString());
            });

            await _dbContext.AddAsync(newRefreshToken!, ct);
            await _dbContext.SaveChangesAsync(ct);
            
            HttpContext.Response.Cookies.Append(AuthConstants.RefreshCookieKey, newRefreshToken!.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Env.IsProduction(),
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiresAt
            });

            await SendOkAsync(new RefreshResponse
            {
                User = refreshToken.User.Adapt<RefreshResponseUser>(),
                Token = newToken,
                RefreshToken = newRefreshToken.Token
            }, ct);
            return;
        }

        _dbContext.Remove(refreshToken);
        await _dbContext.SaveChangesAsync(ct);
        
        ThrowError("Invalid refresh token");
    }
}

internal sealed class RefreshResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public RefreshResponseUser User { get; set; }
}

internal sealed class RefreshResponseUser
{
    public int Id { get; set; }
    public string Username { get; set; }
}

internal sealed class RefreshRequest
{
    public string? RefreshToken { get; set; } = null!;
}