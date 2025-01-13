using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Solves.Endpoints;

internal sealed class DeleteSolveEndpoint(ApplicationDbContext context) : Endpoint<DeleteSolveReqeuest>
{
    private readonly ApplicationDbContext _context = context;

    public override void Configure()
    {
        Delete("solves/{@Id}", s => new {s.Id});
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }

    public override async Task HandleAsync(DeleteSolveReqeuest req, CancellationToken ct)
    {
        var userid = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var solve = await _context.Set<Solve>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);

        if (solve == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (solve.UserId != userid || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }

        _context.Remove(solve);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteSolveReqeuest
{
    public int Id { get; set; }
}