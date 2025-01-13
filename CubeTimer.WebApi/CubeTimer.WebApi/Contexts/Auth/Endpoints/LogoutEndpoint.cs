using FastEndpoints;

namespace CubeTimer.WebApi.Contexts.Auth.Endpoints;
/// <summary>
///     Log out
/// </summary>
internal sealed class LogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("auth/logout");
        
        Description(o =>
        {
            o.ClearDefaultProduces(StatusCodes.Status200OK);
            o.Produces(StatusCodes.Status204NoContent);
        });
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        HttpContext.Response.Cookies.Delete(AuthConstants.RefreshCookieKey);
    }
}