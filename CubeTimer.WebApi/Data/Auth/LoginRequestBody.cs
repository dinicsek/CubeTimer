using System.ComponentModel.DataAnnotations;

namespace CubeTimer.WebApi.Data.Auth;

public class LoginRequestBody
{
 [Required]
 public string Email { get; set; }
 
 [Required]
 public string Password { get; set; }
}