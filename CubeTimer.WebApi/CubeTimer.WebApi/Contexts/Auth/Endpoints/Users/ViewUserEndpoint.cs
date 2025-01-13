using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints.Users;

/// <summary>
///     Get user by id
/// </summary>
internal sealed class ViewUserEndpoint(ApplicationDbContext dbContext)
    : Endpoint<ViewUserRequest, ViewUserResponse, ViewUserMapper>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("users/{@Id}", r => new { UserId = r.Id });

        Description(o =>
        {
            o.Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(ViewUserRequest req, CancellationToken ct)
    {
        var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Id == req.Id, ct);
        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(Map.FromEntity(user), ct);
    }
}

internal sealed class ViewUserRequest
{
    public int Id { get; set; }
}

internal sealed class ViewUserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class ViewUserResponseUserGroup
{
    public int Id { get; set; }
    public string Username { get; set; }
}

internal sealed class ViewUserMapper : Mapper<ViewUserRequest, ViewUserResponse, User>
{
    public override ViewUserResponse FromEntity(User entity)
    {
        return entity.Adapt<ViewUserResponse>();
    }
}
