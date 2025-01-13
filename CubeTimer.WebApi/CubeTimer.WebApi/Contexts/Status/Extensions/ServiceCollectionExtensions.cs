using CubeTimer.WebApi.Contexts.Status.Services.Options;

namespace CubeTimer.WebApi.Contexts.Status.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStatusContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StatusOptions>(configuration.GetSection("Status"));
        
        return services;
    }
}