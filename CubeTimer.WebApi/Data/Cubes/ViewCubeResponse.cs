using CubeTimer.WebApi.Infrastructure.Models;

namespace CubeTimer.WebApi.Data.Cubes;

public class ViewCubeResponse
{
    public int? Id { get; set; }
    
    public CubeEvent CubeEvent { get; set; }
    
    public string CubeType { get; set; }
}
