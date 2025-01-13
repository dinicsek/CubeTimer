using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Cubes.Endpoints;

internal sealed class DeleteCubeEndpoint(ApplicationDbContext dbContext) : Endpoint<DeleteCubeRequest>
{
    private readonly ApplicationDbContext _context = dbContext;

    public override void Configure()
    {
        Delete("cubes/{@Id}", r => new { r.Id });
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }
    
    public override async Task HandleAsync(DeleteCubeRequest req, CancellationToken ct)
    {
        var userid = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var cube = await _context.Set<Cube>().FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        
        if (cube == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        if (cube.UserId != userid || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        _context.Remove(cube);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteCubeRequest
{
    public int Id { get; set; }
}