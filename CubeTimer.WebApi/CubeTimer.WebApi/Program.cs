using System.Text.Json.Serialization;
using CubeTimer.WebApi.Contexts.Auth.Extensions;
using CubeTimer.WebApi.Contexts.Status.Extensions;
using CubeTimer.WebApi.Infrastructure.Extensions;
using CubeTimer.WebApi.Support.Gridify.PostProcessors;
using CubeTimer.WebApi.Support.Json;
using CubeTimer.WebApi.Support.Swagger.Extensions;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FastExpressionCompiler;
using Gridify;
using Mapster;
using Scalar.AspNetCore;

TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

builder.Services.Configure<JwtCreationOptions>( o => o.SigningKey = builder.Configuration["Jwt:SigningKey"]);

builder.Services
    .AddFastEndpoints()
    .AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["Jwt:SigningKey"])
    .AddAuthorization();
    
builder.Services.AddAuthContext(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddStatusContext(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConfiguredSwagger(builder.Configuration);

Console.WriteLine(builder.Configuration["Jwt:SigningKey"]);


var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(o =>
{
    o.Endpoints.RoutePrefix = "api";
    o.Endpoints.ShortNames = true;
    o.Endpoints.Configurator = d => { d.PostProcessor<GridifyExceptionsProcessor>(Order.After); };
    
    o.Versioning.Prefix = "v";
    o.Versioning.DefaultVersion = 1;
    o.Versioning.PrependToRoute = true;
    
    o.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    o.Serializer.Options.PropertyNamingPolicy = new CamelCaseNamingPolicy();
}).UseSwaggerGen(o =>
{
    o.Path = "/openapi/{documentName}.json";
});

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(o =>
    {
        o.DarkMode = true;
        o.Theme = ScalarTheme.DeepSpace;
        o.Title = "CubeTimer API";
        o.ShowSidebar = true;
        o.WithPreferredScheme("Bearer").WithHttpBearerAuthentication(b =>
        {
            b.Token = builder.Configuration["Jwt:SigningKey"];
        });
    });
}

app.Run();

