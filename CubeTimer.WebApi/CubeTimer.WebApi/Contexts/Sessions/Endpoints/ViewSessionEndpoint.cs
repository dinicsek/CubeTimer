using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Sessions.Endpoints;

internal sealed class ViewSessionEndpoint(ApplicationDbContext context) : Endpoint<ViewSessionRequest, ViewSessionResponse, ViewSessionMapper>
{

    private readonly ApplicationDbContext _context = context;
    
    public override void Configure()
    {
        Get("sessions/{@Id}", r => new { SessionId = r.Id });
        
        Description(o =>
        {
            o.Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(ViewSessionRequest req, CancellationToken ct)
    {
        var session = await _context.Set<Session>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);
        
        if (session == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        await SendOkAsync( Map.FromEntity(session), ct);
    }
}

internal sealed class ViewSessionResponse
{
    public int Id { get; set; }
    public string? SessionName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class ViewSessionRequest
{
    public int Id { get; set; }
}

internal sealed class ViewSessionMapper : Mapper<ViewSessionRequest, ViewSessionResponse, Session>
{
    public override ViewSessionResponse FromEntity(Session entity)
    {
        return entity.Adapt<ViewSessionResponse>();
    }
}