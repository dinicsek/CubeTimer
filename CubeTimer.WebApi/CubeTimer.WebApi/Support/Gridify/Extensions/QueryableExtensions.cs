using CubeTimer.WebApi.Support.Gridify.Interfaces;
using Gridify;

namespace CubeTimer.WebApi.Support.Gridify.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyGridifyQuery<T>(this IQueryable<T> query, IGridifyRequest request, IGridifyMapper<T>? mapper = null)
    {
        return query.ApplyGridifyQuery(request.Filter, request.OrderBy, request.Page, request.PageSize, mapper);
    }
    
    public static IQueryable<T> ApplyGridifyQuery<T>(this IQueryable<T> query, string? filter, string? orderBy, int? page, int? pageSize, IGridifyMapper<T>? mapper = null)
    {
        var newQuery = query.ApplyFiltering(filter, mapper).ApplyOrdering(orderBy, mapper);
        
        if (page != null && pageSize != null)
        {
            newQuery = newQuery.ApplyPaging(page.Value, pageSize.Value);
        }
        
        return newQuery;
    }
}