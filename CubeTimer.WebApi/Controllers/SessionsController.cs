using CubeTimer.WebApi.Data.Sessions;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SessionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public SessionsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewSessionResponse>> View([FromRoute] int id)
    {
        var session = await _context.Sessions.FindAsync(id);

        if (session == null)
        {
            return NotFound();
        }
        return Ok(new ViewSessionResponse
        {
            Id = session.Id,
            SessionName = session.SessionName,
            UserId = session.UserId,
            Description = session.Description
        });
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<IndexSessionResponse>>> Index()
    {
        var sessions = await _context.Sessions.ToListAsync();

        return Ok(sessions.Select(s => new IndexSessionResponse
        {
            SessionName = s.SessionName,
            UserId = s.UserId,
            Description = s.Description
        }));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSessionResponse>> Create([FromBody] CreateSessionRequestBody body)
    {
        var session = new Session
        {
            SessionName = body.SessionName,
            UserId = body.UserId,
            Description = body.Description
        };
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(View), new { id = session.Id }, new CreateSessionResponse
        {
            Id = session.Id,
            SessionName = session.SessionName,
            UserId = session.UserId,
            Description = session.Description
        });
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var session = await _context.Sessions.FindAsync(id);

        if (session == null)
        {
            return NotFound();
        }

        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSessionRequestBody body)
    {
        var session = await _context.Sessions.FindAsync(id);

        if (session == null)
        {
            return NotFound();
        }

        session.SessionName = body.SessionName;
        session.UserId = body.UserId;
        session.Description = body.Description;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}