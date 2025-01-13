namespace CubeTimer.WebApi.Support.Gridify.Interfaces;

public interface IGridifyRequest
{
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}