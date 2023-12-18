using System.ComponentModel.DataAnnotations;
using CubeTimer.WebApi.Execption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;

namespace CubeTimer.WebApi.Extensions;

public static class ControllerBaseExtensions
{
    public static async Task Validate(this ControllerBase controller, Func<Task<bool>> predictate, string field, string message)
    {
        if (await predictate())
        {
            throw new ControllerValidationException(field, new[] { message });
        }
    }
}