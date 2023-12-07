using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.Solves;

public class CreateSolveRequestBody : IValidatableObject
{
    [Required]
    [Range(0, int.MaxValue)]
    public int Time { get; set; }
    
    public string? SolveModifier { get; set; }
    
    [Required]
    public string Scramble { get; set; }

    [Required]
    public int SessionId { get; set; }
    
    [Required]
    public int? CubeId { get; set; }

    [Required]
    public int UserId { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SolveModifier != null && !Enum.TryParse<SolveModifier>(SolveModifier, out _))
        {
            yield return new ValidationResult($"The value of {nameof(SolveModifier)} must be one of the following: {string.Join(", ", Enum.GetNames<SolveModifier>())}");
        }
    }
}