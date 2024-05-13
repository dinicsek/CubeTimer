using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CubeTimer.WebApi.Data.Auth;
using CubeTimer.WebApi.Extensions;
using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Models;
using CubeTimer.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CubeTimer.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class AuthController : ControllerBase
{
   private readonly ApplicationDbContext _context;

   private readonly PasswordHasherService _passwordHasherService;
   
   private readonly IConfiguration _config;

   public AuthController(ApplicationDbContext context, PasswordHasherService passwordHasherService, IConfiguration config)
   {
      _context = context;
      _passwordHasherService = passwordHasherService;
      _config = config;
   }
   
  
   
   [EnableCors]
   [HttpPost("register")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult> Register(RegisterRequestBody body)
   {
      await this.Validate(async () => await _context.Users.AnyAsync(u => u.Email == body.Email),
         nameof(RegisterRequestBody.Email), "The given email is already in use.");
      
      string passwordHashed = _passwordHasherService.HashPassword(body.Password);
      var user = new User
      {
         Email = body.Email,
         FirstName = body.FirstName,
         LastName = body.LastName,
         Username = body.Username,
         Password = passwordHashed
      };
      _context.Users.Attach(user);

      await _context.SaveChangesAsync();

      return NoContent();
   }
   [EnableCors]
  [HttpPost("login")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequestBody body)
   {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == body.Email);

      if (user == null)
      {
         return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
         {
            { nameof(LoginRequestBody), new[] { "The given user does not exist." } }
         }));
      }
      
      var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_config["Jwt:Key"]!));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      
      if (!_passwordHasherService.VerifyPassword(body.Password, user.Password))
      {
         return Unauthorized("Invalid password.");
      }

      var tokenDescriptor = new SecurityTokenDescriptor
      {
         Subject = new ClaimsIdentity(new[]
         {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
         }),
         Expires = DateTime.Now.AddDays(30),
         SigningCredentials = credentials,
         Audience = _config["Jwt:Audience"],
         Issuer = _config["Jwt:Issuer"]
      };
      
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      
      return Ok(new LoginResponse
      {
         Token = tokenHandler.WriteToken(token)
      });
   }
   
   [HttpDelete]
   [Authorize]
   [EnableCors]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<ActionResult> Delete([FromBody] DeleteUserRequestBody body)
   {
      var currentUser = await _context.Users.FindAsync(Convert.ToInt32(Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
      
      if (!_passwordHasherService.VerifyPassword(body.Password, currentUser!.Password))
      {
         return Unauthorized("Invalid password.");
      }
      
      _context.Users.Remove(currentUser);
      await _context.SaveChangesAsync();
      return NoContent();
   }
}