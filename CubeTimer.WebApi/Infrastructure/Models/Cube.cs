using CubeTimer.WebApi.Infrastructure.Common;

namespace CubeTimer.WebApi.Infrastructure.Models;

public class Cube : TimestampedEntity
{
    public int Id { get; set; }
    
    public CubeEvent CubeEvent { get; set; }
    
    public string CubeType { get; set; }
    
    public User User { get; set; }
    
    public int UserId { get; set; }
    public List<Solve> Solves { get; set; }
}

public enum CubeEvent
{
ThreeByThree, TwoByTwo, Megaminx, Pyraminx
};


