namespace CubeTimer.WebApi.Data.Cubes;

using CubeTimer.WebApi.Infrastructure.Models;

public class IndexCubeResponse
{
    public int? Id { get; set; }
    
    public CubeEvent CubeEvent { get; set; }
    
    public string CubeType { get; set; }
}
