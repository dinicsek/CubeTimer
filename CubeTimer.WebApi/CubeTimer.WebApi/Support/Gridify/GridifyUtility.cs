using CubeTimer.WebApi.Support.Gridify.Interfaces;

namespace CubeTimer.WebApi.Support.Gridify;

public static class GridifyUtility
{
    public static T CreateGridifyResponse<T, TData>(IEnumerable<TData> data, IGridifyRequest request)
        where T : IGridifyResponse<TData>, new()
    {
        return new T
        {
            Data = data,
            Page = request is { Page: not null, PageSize: not null } ? request.Page : null,
            PageSize = request is { Page: not null, PageSize: not null } ? request.PageSize : null,
        };
    }
}