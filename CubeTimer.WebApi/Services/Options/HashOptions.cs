using System.Security.Cryptography;

namespace CubeTimer.WebApi.Services.Options;

public class HashOptions
{
    public readonly RandomNumberGenerator DefaultRng = RandomNumberGenerator.Create();

    public int PasswordHashIterations { get; set; } = 100000;
    public int PasswordSaltLength { get; set; } = 128 / 8;
    public int PasswordBytesRequested { get; set; } = 256 / 8;
}