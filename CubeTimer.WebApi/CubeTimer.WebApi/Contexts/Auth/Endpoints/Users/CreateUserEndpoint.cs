using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints.Users;

/// <summary>
///     Create a new user
/// </summary>
internal sealed class CreateUserEndpoint(ApplicationDbContext dbContext) : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Post("users");
        AllowAnonymous();
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces<CreateUserResponse>(StatusCodes.Status201Created);
        });
    }
    
    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var user = req.Adapt<User>();

        user.PasswordHashed = BCrypt.Net.BCrypt.HashPassword(req.Password);
        
        await _dbContext.Set<User>().AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        var res = user.Adapt<CreateUserResponse>();

        await SendCreatedAtAsync<ViewUserEndpoint>(new { user.Id }, res, cancellation: ct);
    }
}

internal sealed class CreateUserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

internal sealed class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(r => r.Username).NotEmpty().MustAsync(BeUniqueUsernameAsync)
            .WithMessage("The given Username is taken").WithName("Username");
        RuleFor(r => r.Password).NotEmpty().WithName("Password");
    }
    
    private async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var dbContext = Resolve<ApplicationDbContext>();
        return await dbContext.Set<User>().AllAsync(x => x.Username != username, cancellationToken);
    }
}