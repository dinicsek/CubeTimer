using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Sessions.Endpoints;

internal sealed class DeleteSessionEndpoint(ApplicationDbContext context) : Endpoint<DeleteSessionRequest>
{
    private readonly ApplicationDbContext _context = context;

    public override void Configure()
    {
        Delete("sessions/{@Id}", r => new {r.Id});
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }

    public override async Task HandleAsync(DeleteSessionRequest req, CancellationToken ct)
    {
        var userid = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var session = await _context.Set<Session>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);

        if (session == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (session.UserId != userid || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }

        _context.Remove(session);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteSessionRequest
{
    public int Id { get; set; }
}