using CubeTimer.WebApi.Contexts.Auth.Services;

namespace CubeTimer.WebApi.Contexts.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRefreshService, RefreshService>();

        return services;
    }
}