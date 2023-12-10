using System.Security.Cryptography;
using CubeTimer.WebApi.Data.Auth;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace CubeTimer.WebApi.Controllers;
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class AuthController : ControllerBase
{
   private readonly ApplicationDbContext _context;

   public AuthController(ApplicationDbContext context)
   {
      _context = context;
   }
   
   [HttpPost("register")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult> Register(RegisterRequestBody body)
   {
      byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
      
      string passwordHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
         password: body.Password,
         salt: salt,
         prf: KeyDerivationPrf.HMACSHA256,
         iterationCount: 100000,
         numBytesRequested: 256 / 8));
      
      var user = new User
      {
         Email = body.Email,
         Name = body.Name,
         Username = body.Username,
         Password = passwordHashed
      };
      _context.Users.Attach(user);

      await _context.SaveChangesAsync();

      return NoContent();
   }
   
  /* [HttpPost("login")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<LoginRequestBody>>*/
    
}