using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Infrastructure.Common;

namespace CubeTimer.WebApi.Infrastructure.Models;

public class Solve : TimestampedEntity
{
    [Key]
    public int Id { get; set; }
    
    public TimeSpan Time { get; set; }
    
    public SolveModifier? SolveModifier { get; set; }
    
    public string Scramble { get; set; }
    
    public User User { get; set; }
    
    public Session Session { get; set; }
    
    public Cube Cube { get; set; }
}

public enum SolveModifier { Dnf, PlusTwo };