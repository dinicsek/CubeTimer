﻿namespace CubeTimer.WebApi.Data.Sessions;

public class UpdateSessionRequestBody
{
    public string? SessionName { get; set; }
    
    public string? Description { get; set; }
}