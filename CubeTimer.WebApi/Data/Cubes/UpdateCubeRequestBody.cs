using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.Cubes;

public class UpdateCubeRequestBody
{
    
    [Required]
    public CubeEvent CubeEvent { get; set; }
    
    [Required]
    public string CubeType { get; set; }  
}