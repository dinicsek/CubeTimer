using CubeTimer.WebApi.Data.Solves;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SolvesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public SolvesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndexSolveResponse>> Index()
    {
        var solves = await _context.Solves.ToListAsync();

        return Ok(solves.Select(s => new IndexSolveResponse
        {
            Id = s.Id,
            Time = s.Time,
            SolveModifier = s.SolveModifier,
            Scramble = s.Scramble,
            UserId = s.UserId,
            SessionId = s.SessionId,
            CubeId = s.CubeId
        }));
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewSolveResponse>> View([FromRoute] int id)
    {
        var solve = await _context.Solves.FindAsync(id);

        if (solve == null)
        {
            return NotFound();
        }
        return Ok(new ViewSolveResponse
        {
            Id = solve.Id,
            Time = solve.Time,
            SolveModifier = solve.SolveModifier,
            Scramble = solve.Scramble,
            UserId = solve.UserId,
            SessionId = solve.SessionId,
            CubeId = solve.CubeId
        });
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSolveResponse>> Create([FromBody] CreateSolveRequestBody body)
    {
        if (await _context.Sessions.AllAsync(s => s.Id != body.SessionId))
        {
            return ValidationProblem(new ValidationProblemDetails (new Dictionary<string, string[]>
            {
                {nameof(CreateSolveRequestBody.SessionId), new []{ "The given session does not exist." }}
            }));
        }
        
        if (await _context.Sessions.AllAsync(s => s.Id != body.CubeId))
        {
            return ValidationProblem(new ValidationProblemDetails (new Dictionary<string, string[]>
            {
                {nameof(CreateSolveRequestBody.CubeId), new []{ "The given cube does not exist." }}
            }));
        }
        
        var solve = new Solve
        {
            Time = body.Time,
            SolveModifier = body.SolveModifier != null ? Enum.Parse<SolveModifier>(body.SolveModifier) : null,
            Scramble = body.Scramble,
            SessionId = body.SessionId,
            CubeId = body.CubeId,
            UserId = body.UserId
        };

        _context.Solves.Attach(solve);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(View), new { id = solve.Id }, new CreateSolveResponse
        {
            Id = solve.Id,
            Time = solve.Time,
            SolveModifier = solve.SolveModifier = body.SolveModifier != null ? Enum.Parse<SolveModifier>(body.SolveModifier) : null,
            Scramble = solve.Scramble,
            UserId = solve.UserId,
            SessionId = solve.SessionId,
            CubeId = solve.CubeId
        }); 
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var solve = await _context.Solves.FindAsync(id);
        
        if (solve == null)
        {
            return NotFound();
        }

        _context.Solves.Remove(solve);

        await _context.SaveChangesAsync();
        
        
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSolveRequestBody body)
    {
        if (await _context.Sessions.AllAsync(s => s.Id != body.SessionId))
        {
            return ValidationProblem(new ValidationProblemDetails (new Dictionary<string, string[]>
            {
                {nameof(CreateSolveRequestBody.SessionId), new []{ "The given session does not exist." }}
            }));
        }
        
        if (await _context.Sessions.AllAsync(s => s.Id != body.CubeId))
        {
            return ValidationProblem(new ValidationProblemDetails (new Dictionary<string, string[]>
            {
                {nameof(CreateSolveRequestBody.CubeId), new []{ "The given cube does not exist." }}
            }));
        }
        
        var solve = await _context.Solves.FindAsync(id);

        if (solve == null)
        {
            return NotFound();
        }

        solve.Time = body.Time;
        solve.SolveModifier = body.SolveModifier != null ? Enum.Parse<SolveModifier>(body.SolveModifier) : null;
        solve.Scramble = body.Scramble;
        solve.SessionId = body.SessionId;
        solve.CubeId = body.CubeId;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
}