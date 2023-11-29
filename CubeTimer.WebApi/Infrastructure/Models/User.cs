using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Infrastructure.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : TimestampedEntity
{
    [Key]
    public int Id { get; set; }

    public string Email { get; set; }
    
    public string Name { get; set; }
    
    public string Username { get; set; }
    
    public DateTime? EmailVerifiedAt { get; set; }
    
    public List<Solve> Solves { get; set; }
    
    public List<Session> Sessions { get; set; }
}