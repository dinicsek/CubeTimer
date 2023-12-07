namespace CubeTimer.WebApi.Data.Sessions;

public class UpdateSessionRequestBody
{
    public int Id { get; set; }
    
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
    
    public int UserId { get; set; }
}