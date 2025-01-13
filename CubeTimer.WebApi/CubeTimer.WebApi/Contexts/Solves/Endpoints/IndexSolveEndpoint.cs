using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using CubeTimer.WebApi.Support.Gridify;
using CubeTimer.WebApi.Support.Gridify.Extensions;
using CubeTimer.WebApi.Support.Gridify.Interfaces;
using FastEndpoints;
using Gridify;
using Microsoft.EntityFrameworkCore;
using Mapster;


namespace CubeTimer.WebApi.Contexts.Solves.Endpoints;

internal sealed class IndexSolveEndpoint(ApplicationDbContext context) : Endpoint<IndexSolvesRequest, IndexSolvesResponse>
{
    private readonly ApplicationDbContext _context = context;

    public override void Configure()
    {
        Get("solves");
    }

    public override async Task HandleAsync(IndexSolvesRequest req, CancellationToken ct)
    {
        var mapper = new GridifyMapper<Solve>().GenerateMappings()
            .RemoveMap(nameof(Entities.Solve.UserId))
            .RemoveMap(nameof(Entities.Solve.User))
            .RemoveMap(nameof(Entities.Solve.SessionId))
            .RemoveMap(nameof(Entities.Solve.Session))
            .RemoveMap(nameof(Entities.Solve.CubeId))
            .RemoveMap(nameof(Entities.Solve.Cube));

        var filteredSolves = await _context.Set<Solve>().ApplyGridifyQuery(req, mapper).ToListAsync(ct);

        await SendOkAsync(GridifyUtility.CreateGridifyResponse<IndexSolvesResponse, IndexSolveResponseSolve>(filteredSolves.Adapt<IEnumerable<IndexSolveResponseSolve>>(), req), ct);
    }
}

internal sealed class IndexSolvesResponse: IGridifyResponse<IndexSolveResponseSolve>
{
    public IEnumerable<IndexSolveResponseSolve> Data { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexSolvesRequest : IGridifyRequest
{
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexSolveResponseSolve
{
    public int Id { get; set; }
    public int Time { get; set; }
    public SolveModifier? SolveModifier { get; set; }
    public string Scramble { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}