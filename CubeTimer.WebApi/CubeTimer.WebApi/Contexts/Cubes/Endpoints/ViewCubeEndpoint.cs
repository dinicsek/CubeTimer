using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Cubes.Endpoints;

internal sealed class ViewCubeEndpoint(ApplicationDbContext dbContext) : Endpoint<ViewCubeRequest, ViewCubeResponse, ViewCubeMapper>
{
    private readonly ApplicationDbContext _context = dbContext;
    
    public override void Configure()
    {
        Get("cubes/{@Id}", r => new { CubeId = r.Id });
        
        Description(o =>
        {
            o.Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(ViewCubeRequest req, CancellationToken ct)
    {
        var cube = await _context.Set<Cube>().FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        if (cube == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(Map.FromEntity(cube), ct);
    }
}

internal sealed class ViewCubeMapper : Mapper<ViewCubeRequest, ViewCubeResponse, Cube>
{
    public override ViewCubeResponse FromEntity(Cube entity)
    {
        return entity.Adapt<ViewCubeResponse>();
    }
}

internal sealed class ViewCubeResponse
{
    public int Id { get; set; }
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class ViewCubeRequest
{
    public int Id { get; set; }
}

internal sealed class ViewCubeResponseUserGroup
{
    public int Id { get; set; }
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
}