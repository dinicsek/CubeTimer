using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CubeTimer.WebApi.Data.Cubes;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class CubesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public CubesController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewCubeResponse>> View([FromRoute] int id)
    {
        var cube = await _context.Cubes.FindAsync(id);

        if (cube == null)
        {
            return NotFound();
        }
        return Ok(new ViewCubeResponse
        {
            Id = cube.Id,
            CubeEvent = cube.CubeEvent,
            CubeType = cube.CubeType
        });
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<IndexCubeResponse>>> Index()
    {
        var cubes = await _context.Cubes.ToListAsync();

        return Ok(cubes.Select(c => new IndexCubeResponse
        {
            Id = c.Id,
            CubeEvent = c.CubeEvent,
            CubeType = c.CubeType
        }));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCubeResponse>> Create([FromBody] CreateCubeRequestBody body)
    {
        var cube = new Cube
        {
            CubeEvent = body.CubeEvent,
            CubeType = body.CubeType,
            UserId = Convert.ToInt32( User.FindFirst(ClaimTypes.NameIdentifier)?.Value)

        };
        await _context.Cubes.AddAsync(cube);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(View), new { id = cube.Id }, new CreateCubeResponse
        {
            CubeEvent = cube.CubeEvent,
            CubeType = cube.CubeType
        });
    }
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var cube = await _context.Cubes.FindAsync(id);

        if (cube == null)
        {
            return NotFound();
        }

        _context.Cubes.Remove(cube);

        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    [HttpPatch("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCubeRequestBody body)
    {
        var cube = await _context.Cubes.FindAsync(id);
        
        if (cube == null)
        {
            return NotFound();
        }
        
        cube.CubeEvent = body.CubeEvent;
        cube.CubeType = body.CubeType;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
}