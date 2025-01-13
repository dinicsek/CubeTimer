using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints;

/// <summary>
///     Get the control object
/// </summary>
internal sealed class ControlEndpoint : EndpointWithoutRequest<ControlResponse>
{
    public override void Configure()
    {
        Get("auth/control");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new ControlResponse
        {
            User = new ControlResponseUser
            {
                Id = Convert.ToInt32(User.ClaimValue(ClaimTypes.NameIdentifier)),
                Username = User.ClaimValue(ClaimTypes.Name)!,
            }
        }, ct);
    }
}

internal sealed class ControlResponse
{
    public ControlResponseUser User { get; set; }
}

internal sealed class ControlResponseUser
{
    public int Id { get; set; }
    public string Username { get; set; }
}