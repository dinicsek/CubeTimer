namespace CubeTimer.WebApi.Execption;

public class ControllerValidationException : Exception
{
    public string Field { get; init; }
    public string[] Errors { get; init; }
    
    public ControllerValidationException(string field, string[] errors)
    {
        Field = field;
        Errors = errors;
    }
}