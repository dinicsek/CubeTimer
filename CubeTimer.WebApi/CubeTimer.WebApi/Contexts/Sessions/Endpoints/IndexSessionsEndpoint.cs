using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using CubeTimer.WebApi.Support.Gridify;
using CubeTimer.WebApi.Support.Gridify.Extensions;
using CubeTimer.WebApi.Support.Gridify.Interfaces;
using FastEndpoints;
using Gridify;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Sessions.Endpoints;

internal sealed class IndexSessionsEndpoint(ApplicationDbContext context) : Endpoint<IndexSessionsRequest, IndexSessionsResponse>
{

    private readonly ApplicationDbContext _context = context;

    public override void Configure()
    {
        Get("sessions");
    }

    public override async Task HandleAsync(IndexSessionsRequest req, CancellationToken ct)
    {
        var mapper = new GridifyMapper<Session>().GenerateMappings()
            .RemoveMap(nameof(Entities.Session.UserId))
            .RemoveMap(nameof(Entities.Session.Solves))
            .RemoveMap(nameof(Entities.Session.User));

        var filteredSessions = await _context.Set<Session>().ApplyGridifyQuery(req, mapper).ToListAsync(ct);

        await SendOkAsync(
            GridifyUtility.CreateGridifyResponse<IndexSessionsResponse, IndexSessionsResponseSession>(
                filteredSessions.Adapt<IEnumerable<IndexSessionsResponseSession>>(), req), ct);
    }
}

internal sealed class IndexSessionsRequest : IGridifyRequest
{
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexSessionsResponse : IGridifyResponse<IndexSessionsResponseSession>
{
    public IEnumerable<IndexSessionsResponseSession> Data { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexSessionsResponseSession
{
    public int Id { get; set; }
    public string? SessionName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}