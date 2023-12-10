using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.Leaderboard;

public class ViewLeaderboardResponse
{
    public int Id { get; set; }

    public int Time { get; set; }

    public int UserId { get; set; }

    public int? CubeId { get; set; }

    public CubeEvent CubeEvent { get; set; }
    
    public string Scramble { get; set; }
    
    public DateTime CreatedAt { get; set; }
}