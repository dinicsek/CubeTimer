using System.ComponentModel.DataAnnotations;

namespace CubeTimer.WebApi.Data.Sessions;

public class CreateSessionRequestBody
{
    [Required]
    public string? SessionName { get; set; } = null!;
    
    [Required]
    public int UserId { get; set; }
    
    public string? Description { get; set; }
}