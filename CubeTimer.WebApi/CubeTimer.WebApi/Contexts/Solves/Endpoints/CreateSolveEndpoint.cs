using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Mapster;

namespace CubeTimer.WebApi.Contexts.Solves.Endpoints;

internal sealed class CreateSolveEndpoint(ApplicationDbContext context) : Endpoint<CreateSolveRequest, CreateSolveResponse>
{

    private readonly ApplicationDbContext _context = context;
    
    public override void Configure()
    {
        Post("solves");
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status201Created);
        });
    }

    public override async Task HandleAsync(CreateSolveRequest req, CancellationToken ct)
    {
        var solve = req.Adapt<Solve>();
        solve.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        solve.SessionId = req.SessionId;
        solve.CubeId = req.CubeId;

        await _context.Set<Solve>().AddAsync(solve, ct);
        await _context.SaveChangesAsync(ct);

        var res = solve.Adapt<CreateSolveResponse>();

        await SendCreatedAtAsync<ViewSolveEndpoint>( new {solve.Id}, res,cancellation: ct);
    }
}

internal sealed class CreateSolveResponse
{
    public int Id { get; set; }

    public int Time { get; set; }

    public SolveModifier? SolveModifier { get; set; }

    public string Scramble { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class CreateSolveRequest
{
    public int Time { get; set; }
    
    public string Scramble { get; set; }
    
    public int SessionId { get; set; }
    
    public int CubeId { get; set; }
}