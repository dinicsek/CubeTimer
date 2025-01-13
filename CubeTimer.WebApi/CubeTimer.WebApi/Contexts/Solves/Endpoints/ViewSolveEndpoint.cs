using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Solves.Endpoints;

internal sealed class ViewSolveEndpoint(ApplicationDbContext context) : Endpoint<ViewSolveRequest, ViewSolveResponse, ViewSolveMapper>
{
    private readonly ApplicationDbContext _context = context;

    public override void Configure()
    {
        Get("solves/{@Id}", r => new { SessionId = r.Id });
        
        Description(o =>
        {
            o.Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(ViewSolveRequest req, CancellationToken ct)
    {
        var solve = await _context.Set<Solve>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);

        if (solve == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(Map.FromEntity(solve), ct);
    }
}

internal sealed class ViewSolveMapper : Mapper<ViewSolveRequest, ViewSolveResponse, Solve>
{
    public override ViewSolveResponse FromEntity(Solve entity)
    {
        return entity.Adapt<ViewSolveResponse>();
    }
}

internal sealed class ViewSolveResponse
{
    public int Id { get; set; }
    public int Time { get; set; }
    public SolveModifier? SolveModifier { get; set; }
    public string Scramble { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class ViewSolveRequest
{
    public int Id { get; set; }
}