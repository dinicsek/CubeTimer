namespace CubeTimer.WebApi.Data.Sessions;

public class CreateSessionResponse
{
    public int Id { get; set; }
    
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
    
    public int UserId { get; set; }
}