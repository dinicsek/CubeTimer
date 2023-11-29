using CubeTimer.WebApi.Infrastructure;
using CubeTimer.WebApi.Infrastructure.Interceptor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UpdateTimestampsInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, o) =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    o.AddInterceptors(sp.GetRequiredService<UpdateTimestampsInterceptor>());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();