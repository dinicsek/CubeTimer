using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Contexts.Auth.Services;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints;

internal sealed class LoginEndpoint(ApplicationDbContext dbContext, IRefreshService refreshService) : Endpoint<LoginRequest, LoginResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IRefreshService _refreshService = refreshService;

    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
        Throttle(30, 60);
        
        Options(o =>
        {
            o.WithTags("Auth");
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Username == req.Username, ct);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHashed))
        {
            ThrowError(r => r.Username, "Invalid username or password");
        }

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = Config["Jwt:SigningKey"];
            o.Issuer = Config["Jwt:Issuer"];
            o.ExpireAt = DateTime.Now.AddHours(1);

            o.User[ClaimTypes.NameIdentifier] = user.Id.ToString();
            o.User[ClaimTypes.Name] = user.Username;
            o.User.Roles.Add(user.Role.ToString());
        });

        if (req.RememberMe &&
            _refreshService.TryCreateRefreshToken(user.Id, DateTime.UtcNow.AddMonths(1), null, out var refreshToken))
        {
            _dbContext.Set<RefreshToken>().Add(refreshToken!);
            await _dbContext.SaveChangesAsync(ct);
            
            HttpContext.Response.Cookies.Append(AuthConstants.RefreshCookieKey, refreshToken!.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Env.IsProduction(),
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiresAt
            });

            await SendOkAsync(new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                User = user.Adapt<LoginResponseUser>()
            }, ct);
            return;
        }
        
        await SendOkAsync(new LoginResponse
        {
            Token = token,
            User = user.Adapt<LoginResponseUser>()
        }, ct);
    }
}

internal sealed class LoginResponse
{
    public LoginResponseUser User { get; set; }
    public string Token { get; set; }
    public string? RefreshToken { get; set; }
}

internal sealed class LoginResponseUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

internal sealed class LoginValidator : Validator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithName("Username");
        RuleFor(x => x.Password).NotEmpty().WithName("Password");
    }
}