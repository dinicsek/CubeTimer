using CubeTimer.WebApi.Data.Leaderboard;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LeaderboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public LeaderboardController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ViewLeaderboardResponse>>> ViewLeaderboard([FromQuery] string cubeEvent)
    {
        var solves = await _context.Solves.Include(s => s.Cube).Where(s => s.SolveModifier != SolveModifier.Dnf)
            .OrderBy(s => s.Time).Take(10).ToListAsync();

        return Ok(solves.Select(s => new ViewLeaderboardResponse
        {
            Id = s.Id,
            Time = s.Time,
            UserId = s.UserId,
            CubeId = s.CubeId,
            CubeEvent = s.Cube!.CubeEvent,
            Scramble = s.Scramble,
            CreatedAt = s.CreatedAt
        }));
    }
}