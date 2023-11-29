using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Infrastructure.Models;

public class Session : TimestampedEntity
{
    [Key]
    public int Id { get; set; }
    
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
    
    public User User { get; set; }
    
    public List<Solve> Solves { get; set; }
}