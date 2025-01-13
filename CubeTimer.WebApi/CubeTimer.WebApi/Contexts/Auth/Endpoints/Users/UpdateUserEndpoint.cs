using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints.Users;

/// <summary>
///     Update user by id
/// </summary>
internal sealed class UpdateUserEndpoint(ApplicationDbContext dbContext) : Endpoint<UpdateUserRequest>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Patch("users/{@id}", r => new { r.Id });

        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        if (req.Id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) || !User.IsInRole("Admin"))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        var user = await _dbContext.Set<User>().Where(u => u.Id == req.Id)
            .FirstOrDefaultAsync(ct);

        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (user.Username != req.Username &&
            await _dbContext.Set<User>().AnyAsync(x => x.Username == req.Username, ct))
            ThrowError(r => r.Username, "The given username is already taken");

        req.Adapt(user);

        await _dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
    
internal sealed class UpdateUserValidator : Validator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(r => r.Username).NotEmpty().WithName("Name");
    }
}