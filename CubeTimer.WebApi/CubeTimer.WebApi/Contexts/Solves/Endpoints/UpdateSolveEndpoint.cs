using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Solves.Entities;
using CubeTimer.WebApi.Contexts.Solves.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Solves.Endpoints;

internal sealed class UpdateSolveEndpoint(ApplicationDbContext context) : Endpoint<UpdateSolveRequest>
{
    private readonly ApplicationDbContext _context = context;
    
    public override void Configure()
    {
        Patch("solves/{@Id}", r => new { r.Id });
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }

    public override async Task HandleAsync(UpdateSolveRequest req, CancellationToken ct)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var solve = await _context.Set<Solve>().FirstOrDefaultAsync(s => s.Id == req.Id, ct);
        
        if (solve == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (solve.UserId != userId || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        req.Adapt(solve);

        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class UpdateSolveRequest
{
    public int Id { get; set; }

    public int Time { get; set; }

    public SolveModifier? SolveModifier { get; set; }

    public string Scramble { get; set; }
}

internal sealed class UpdateSolveValidator : Validator<UpdateSolveRequest>
{
    public UpdateSolveValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithName("Id");
        RuleFor(r => r.Time).NotEmpty().WithName("Time");
        RuleFor(r => r.SolveModifier).NotEmpty().WithName("SolveModifier").IsInEnum();
        RuleFor(r => r.Scramble).NotEmpty().WithName("Scramble");

        
    }
}