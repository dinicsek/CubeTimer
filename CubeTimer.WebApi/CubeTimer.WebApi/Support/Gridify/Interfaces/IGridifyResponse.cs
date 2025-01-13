namespace CubeTimer.WebApi.Support.Gridify.Interfaces;

public interface IGridifyResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}