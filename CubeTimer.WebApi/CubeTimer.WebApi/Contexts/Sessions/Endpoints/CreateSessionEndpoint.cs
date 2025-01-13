using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Sessions.Entities;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Sessions.Endpoints;

internal sealed class CreateSessionEndpoint(ApplicationDbContext context) : Endpoint<CreateSessionRequest, CreateSessionResponse>
{
    private readonly ApplicationDbContext _context;

    public override void Configure()
    {
        Post("sessions");
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status201Created);
        });
    }

    public override async Task HandleAsync(CreateSessionRequest req, CancellationToken ct)
    {
        var session = req.Adapt<Session>();
        session.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        await _context.Set<Session>().AddAsync(session, ct);
        await _context.SaveChangesAsync(ct);
        
        var res = session.Adapt<CreateSessionResponse>();
        
        await SendCreatedAtAsync<ViewSessionEndpoint>(new { session.Id }, res, cancellation: ct);
    }
}

internal sealed class CreateSessionResponse
{
    public int Id { get; set; }
    public string? SessionName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
internal sealed class CreateSessionRequest
{
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
}