using FastEndpoints;
using FluentValidation.Results;
using Gridify;

namespace CubeTimer.WebApi.Support.Gridify.PostProcessors;

public class GridifyExceptionsProcessor : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext ctx, CancellationToken ct)
    {
        if (!ctx.HasExceptionOccurred)
        {
            return;
        }

        if (ctx.ExceptionDispatchInfo.SourceException.GetType() == typeof(GridifyFilteringException))
        {
            ctx.MarkExceptionAsHandled();
            await ctx.HttpContext.Response.SendErrorsAsync([new ValidationFailure("Filter", "Invalid 'Filter' value")],
                cancellation: ct);
            return;
        }
        
        if (ctx.ExceptionDispatchInfo.SourceException.GetType() == typeof(GridifyOrderingException))
        {
            ctx.MarkExceptionAsHandled();
            await ctx.HttpContext.Response.SendErrorsAsync(
                [new ValidationFailure("OrderBy", "Invalid 'OrderBy' value")], cancellation: ct);
            return;
        }
        
        if (ctx.ExceptionDispatchInfo.SourceException.GetType() == typeof(GridifyMapperException))
        {
            ctx.MarkExceptionAsHandled();
            await ctx.HttpContext.Response.SendErrorsAsync(
            [
                new ValidationFailure("Filter", "Invalid 'Filter' or 'OrderBy' value"),
                new ValidationFailure("OrderBy", "Invalid 'Filter' or 'OrderBy' value")
            ], cancellation: ct);
            return;
        }

        ctx.ExceptionDispatchInfo.Throw();
    }
}