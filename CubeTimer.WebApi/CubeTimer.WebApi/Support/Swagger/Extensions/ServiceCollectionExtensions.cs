using CubeTimer.WebApi.Support.Gridify.OperationProcessors;
using FastEndpoints.Swagger;

namespace CubeTimer.WebApi.Support.Swagger.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.SwaggerDocument(o =>
        {
            o.MaxEndpointVersion = 1;
            o.DocumentSettings = s =>
            {
                s.Title = "CubeTimer API";
                s.Version = "v1";

                s.OperationProcessors.Add(new GridifyOperationProcessor());
            };
            o.ShortSchemaNames = true;
        });

        return services;
    }
}