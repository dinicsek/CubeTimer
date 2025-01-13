namespace CubeTimer.WebApi.Infrastructure.Database.Common;

public class TimestampedEntity
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}