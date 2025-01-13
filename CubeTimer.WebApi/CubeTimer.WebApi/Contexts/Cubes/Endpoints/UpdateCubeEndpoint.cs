using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Auth.Entities.Enums;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Contexts.Cubes.Endpoints;

internal sealed class UpdateCubeEndpoint(ApplicationDbContext dbContext) : Endpoint<UpdateCubeRequest>
{
    private readonly ApplicationDbContext _context = dbContext;

    public override void Configure()
    {
        Patch("cubes/{@id}", r => new { r.Id });

        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
            o.Produces(StatusCodes.Status404NotFound);
            o.Produces(StatusCodes.Status403Forbidden);
        });
    }

    public override async Task HandleAsync(UpdateCubeRequest req, CancellationToken ct)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var cube = await _context.Set<Cube>().FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        
        if (cube == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (cube.UserId != userId || !User.IsInRole(Role.Admin.ToString()))
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        req.Adapt(cube);

        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class UpdateCubeRequest
{
    public int Id { get; set; }
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
}

internal sealed class UpdateCubeValidator : Validator<UpdateCubeRequest>
{
    public UpdateCubeValidator()
    {
        RuleFor(x => x.CubeEvent).IsInEnum().WithName("Cube event");
        RuleFor(x => x.CubeMake).NotEmpty().WithName("Cube make");
    }
}