using CubeTimer.WebApi.Contexts.Status.Services.Options;
using CubeTimer.WebApi.Infrastructure.Database;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace CubeTimer.WebApi.Contexts.Status.Endpoints;

internal sealed class ViewStatusEndpoint(ApplicationDbContext context, IOptions<StatusOptions> options) : EndpointWithoutRequest<ViewStatusResponse>
{
    private readonly ApplicationDbContext _context = context;
    private readonly StatusOptions _options = options.Value;
    
    public override void Configure()
    {
        Get("status");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var status = new ViewStatusResponseServiceStatus
        {
            Database = await _context.Database.CanConnectAsync(ct)
        };
        
        var response = new ViewStatusResponse
        {
            Ready = status is { Database: true},
            ServiceStatus = status,
            ServiceInformation = new ViewStatusResponseServiceInformation
            {
                WhoAmI = _options.WhoAmI,
                Version = _options.Version,
                Developers = _options.Developers
            }
        };
        
        await SendOkAsync(response, ct);
    }
}

internal sealed class ViewStatusResponse
{
    public bool Ready { get; set; }
    public ViewStatusResponseServiceStatus ServiceStatus { get; set; }
    public ViewStatusResponseServiceInformation ServiceInformation { get; set; }

}

internal sealed class ViewStatusResponseServiceInformation
{
    public string WhoAmI { get; set; }
    public string Version { get; set; }
    public List<string> Developers { get; set; }
}

internal sealed class ViewStatusResponseServiceStatus
{
    public bool Database { get; set; }
}