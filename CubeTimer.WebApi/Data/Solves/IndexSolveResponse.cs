using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.Solves;

public class IndexSolveResponse
{
    public int Id { get; set; }
    
    public int Time { get; set; }
    
    public SolveModifier? SolveModifier { get; set; }
    
    public string Scramble { get; set; }
    
    public int UserId { get; set; }
    
    public int SessionId { get; set; }
    
    public int? CubeId { get; set; }   
}