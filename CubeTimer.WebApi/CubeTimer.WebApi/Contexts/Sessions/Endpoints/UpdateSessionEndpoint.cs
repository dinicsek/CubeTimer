using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Sessions.Endpoints;

internal sealed class UpdateSessionEndpoint(ApplicationDbContext context) : Endpoint<UpdateSessionRequest>
{
    
    private readonly ApplicationDbContext _context = context;
    
    public override void Configure()
    {
        Patch("sessions/{@Id}", r => new { r.Id });
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }

    public override async Task HandleAsync(UpdateSessionRequest req, CancellationToken ct)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var session = await _context.Set<Session>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);
        
        if (session == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (session.UserId != userId || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        req.Adapt(session);

        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class UpdateSessionRequest
{
    public int Id { get; set; }
    
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
}

internal sealed class UpdateSessionValidator : Validator<UpdateSessionRequest>
{
    public UpdateSessionValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}