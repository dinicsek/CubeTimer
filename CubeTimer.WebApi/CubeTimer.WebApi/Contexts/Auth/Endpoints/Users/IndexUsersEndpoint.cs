using CubeTimer.WebApi.Contexts.Auth.Entities;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using CubeTimer.WebApi.Support.Gridify;
using CubeTimer.WebApi.Support.Gridify.Extensions;
using CubeTimer.WebApi.Support.Gridify.Interfaces;
using FastEndpoints;
using Gridify;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints.Users;

/// <summary>
///     List all users
/// </summary>
internal sealed class IndexUsersEndpoint(ApplicationDbContext dbContext) : Endpoint<IndexUsersRequest, IndexUsersResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public override void Configure()
    {
        Get("users");
        Roles(Role.Admin.ToString());
    }

    public override async Task HandleAsync(IndexUsersRequest req, CancellationToken ct)
    {
        if (!User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var mapper = new GridifyMapper<User>().GenerateMappings()
            .RemoveMap(nameof(Entities.User.PasswordHashed))
            .RemoveMap(nameof(Entities.User.Role));

        var filteredUsers = await _dbContext.Set<User>().ApplyGridifyQuery(req, mapper).ToListAsync(ct);

        await SendOkAsync(
            GridifyUtility.CreateGridifyResponse<IndexUsersResponse, IndexUsersResponseUser>(
                filteredUsers.Adapt<IEnumerable<IndexUsersResponseUser>>(), req), ct);
    }
}

internal class IndexUsersResponse : IGridifyResponse<IndexUsersResponseUser>
{
    public IEnumerable<IndexUsersResponseUser> Data { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexUsersRequest : IGridifyRequest
{
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexUsersResponseUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}