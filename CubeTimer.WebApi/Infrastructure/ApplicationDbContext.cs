using CubeTimer.WebApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CubeTimer.WebApi.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Solve>().HasOne<Cube>(s => s.Cube).WithMany(c => c.Solves).HasForeignKey(s => s.CubeId).OnDelete(DeleteBehavior.SetNull);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Solve> Solves { get; set; }
    
    public DbSet<Session> Sessions { get; set; }
    
    public DbSet<Cube> Cubes { get; set; }
}