using System.Security.Claims;
using CubeTimer.WebApi.Contexts.Cubes.Entities;
using CubeTimer.WebApi.Contexts.Cubes.Entities.Enums;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using FluentValidation;
using Mapster;

namespace CubeTimer.WebApi.Contexts.Cubes.Endpoints;

internal sealed class CreateCubeEndpoint(ApplicationDbContext dbContext) : Endpoint<CreateCubeRequest, CreateCubeResponse>
{
    private readonly ApplicationDbContext _context = dbContext;

    public override void Configure()
    {
        Post("cubes");
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces<CreateCubeResponse>(StatusCodes.Status201Created);
        });
    }

    public override async Task HandleAsync(CreateCubeRequest req, CancellationToken ct)
    {
        var cube = req.Adapt<Cube>();
        cube.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        await _context.Set<Cube>().AddAsync(cube, ct);
        await _context.SaveChangesAsync(ct);
        
        var res = cube.Adapt<CreateCubeResponse>();
        
        await SendCreatedAtAsync<ViewCubeEndpoint>(new { cube.Id }, res, cancellation: ct);
    }
}

internal sealed class CreateCubeResponse
{
    public int Id { get; set; }
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

internal sealed class CreateCubeRequest
{
    public CubeEvent CubeEvent { get; set; }
    public string CubeMake { get; set; }
}

internal sealed class CreateCubeValidator : Validator<CreateCubeRequest>
{
    public CreateCubeValidator()
    {
        RuleFor(x => x.CubeEvent).IsInEnum().WithName("Cube Event");
        RuleFor(x => x.CubeMake).NotEmpty().WithName("Cube Make");
    }
    
    
}