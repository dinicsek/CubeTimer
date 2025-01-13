using CubeTimer.WebApi.Support.Gridify.Interfaces;
using FastEndpoints;
using NJsonSchema;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace CubeTimer.WebApi.Support.Gridify.OperationProcessors;

public class GridifyOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext ctx)
    {
        if (ctx is not AspNetCoreOperationProcessorContext)
        {
            return true;
        }
        
        var endpointDefinition = ((AspNetCoreOperationProcessorContext)ctx).ApiDescription.ActionDescriptor.EndpointMetadata.OfType<EndpointDefinition>().FirstOrDefault();

        if (endpointDefinition == null)
        {
            return true;
        }
        
        var requestType = endpointDefinition.ReqDtoType;

        if (requestType.IsAssignableTo(typeof(IGridifyRequest)))
        {
            ctx.OperationDescription.Operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = JsonSchema.FromType<ErrorResponse>()
                    }
                }
            });
        }
        
        return true;
    }
}