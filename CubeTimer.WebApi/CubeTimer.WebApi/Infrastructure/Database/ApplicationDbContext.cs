using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder); 
    }
}