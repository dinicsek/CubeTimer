using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints.Users;

/// <summary>
///     Delete user by id
/// </summary>
internal sealed class DeleteUserEndpoint(ApplicationDbContext dbContext) : Endpoint<DeleteUserRequest>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public override void Configure()
    {
        Delete("users/{@Id}", r => new { r.Id });
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }
    
    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        if (req.Id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) || !User.IsInRole("Admin"))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        var user = await _dbContext.Set<User>().FindAsync(req.Id, ct);
        
        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteUserRequest
{
    public int Id { get; set; }
}

