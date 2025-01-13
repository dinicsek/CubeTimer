using CubeTimer.WebApi.Contexts.Auth.Entities;

namespace CubeTimer.WebApi.Contexts.Auth.Services;

public interface IRefreshService
{
    /// <summary>
    ///     Tries to create a refresh token for the given user. It does not add the token to the database.
    /// </summary>
    /// <param name="userId">The user for whom the refresh token should be created.</param>
    /// <param name="expireAt">The date time at which the token should be considered expired.</param>
    /// <param name="parentRefreshToken">The current refresh token of the user.</param>
    /// <param name="refreshToken">The newly created refresh token.</param>
    /// <returns>True if a new token could be created. False if not, meaning the parent token already has a child token.</returns>
    public bool TryCreateRefreshToken(int userId, DateTime expireAt, string? parentRefreshToken,
        out RefreshToken? refreshToken);

    /// <summary>
    ///     Validates the given refresh token.
    /// </summary>
    /// <param name="token">The base64 string representation of the refresh token.</param>
    /// <param name="refreshToken">The <see cref="RefreshToken" /> entity corresponding to the string token.</param>
    /// <returns>Whether the refresh token is still valid.</returns>
    public bool ValidateRefreshToken(string token, out RefreshToken? refreshToken);
}