using System.ComponentModel.DataAnnotations;

namespace CubeTimer.WebApi.Data.Auth;

public class RegisterRequestBody
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    
    public string LastName { get; set; }
    
    [Required]
    public string Username { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")]
    public string Password { get; set; }
}