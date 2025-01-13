using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using CubeTimer.WebApi.Support.Gridify;
using CubeTimer.WebApi.Support.Gridify.Extensions;
using CubeTimer.WebApi.Support.Gridify.Interfaces;
using FastEndpoints;
using Gridify;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Cubes.Endpoints;

internal sealed class IndexCubesEndpoint(ApplicationDbContext dbContext) : Endpoint<IndexCubesRequest, IndexCubesResponse>
{
    private readonly ApplicationDbContext _context = dbContext;
    
    public override void Configure()
    {
        Get("cubes");
    }
    
    public override async Task HandleAsync(IndexCubesRequest req, CancellationToken ct)
    {
        var mapper = new GridifyMapper<Cube>().GenerateMappings()
            .RemoveMap(nameof(Entities.Cube.UserId))
            .RemoveMap(nameof(Entities.Cube.Solves))
            .RemoveMap(nameof(Entities.Cube.User));

        var filteredCubes = await _context.Set<Cube>().ApplyGridifyQuery(req, mapper).ToListAsync(ct);

        await SendOkAsync(
            GridifyUtility.CreateGridifyResponse<IndexCubesResponse, IndexCubesResponseCube>(
                filteredCubes.Adapt<IEnumerable<IndexCubesResponseCube>>(), req), ct);
    }
}

internal sealed class IndexCubesRequest : IGridifyRequest
{
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexCubesResponse : IGridifyResponse<IndexCubesResponseCube>
{
    public IEnumerable<IndexCubesResponseCube> Data { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

internal sealed class IndexCubesResponseCube
{
    public int Id { get; set; }
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}