using CubeTimer.WebApi.Infrastructure.Database;
using CubeTimer.WebApi.Infrastructure.Database.Interceptor;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("DefaultConnection");
       
       if (string.IsNullOrEmpty(connectionString))
       {
           throw new InvalidOperationException("Connection string not found.");
       }

       services.AddSingleton<UpdateTimestampsInterceptor>();

       services.AddDbContext<ApplicationDbContext>((sp, o) =>
       {
           o.UseNpgsql(connectionString);
           o.AddInterceptors(sp.GetRequiredService<UpdateTimestampsInterceptor>());
       });

       return services;
    }
}