using System.Security.Cryptography;
using CubeTimer.WebApi.Services.Options;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace CubeTimer.WebApi.Services;

public class PasswordHasherService
{
    
    private readonly HashOptions _hashOptions;
    private readonly RandomNumberGenerator _rng;
    
    public PasswordHasherService(IOptions<HashOptions> hashOptions)
    {
        _rng = hashOptions.Value.DefaultRng;

        if (hashOptions.Value.PasswordHashIterations < 1)
            throw new ArgumentException("Password hash iterations must be greater than 0", nameof(hashOptions));

        if (hashOptions.Value.PasswordSaltLength < 1)
            throw new ArgumentException("Password salt length must be greater than 0", nameof(hashOptions));

        if (hashOptions.Value.PasswordBytesRequested < 1)
            throw new ArgumentException("Password bytes requested must be greater than 0", nameof(hashOptions));

        _hashOptions = hashOptions.Value;
    }
    public string HashPassword(string password)
    {
        var salt = new byte[_hashOptions.PasswordSaltLength];
        _rng.GetBytes(salt);

        var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512,
            _hashOptions.PasswordHashIterations,
            _hashOptions.PasswordBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subkey.Length];
        outputBytes[0] = 0x01;
        WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
        WriteNetworkByteOrder(outputBytes, 5, (uint)_hashOptions.PasswordHashIterations);
        WriteNetworkByteOrder(outputBytes, 9, (uint)_hashOptions.PasswordSaltLength);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 13 + _hashOptions.PasswordSaltLength, subkey.Length);

        return Convert.ToBase64String(outputBytes);
    }
    public bool VerifyPassword(string password, string hash)
    {
        var decodedHash = Convert.FromBase64String(hash);

        if (decodedHash.Length == 0)
            return false;

        if (decodedHash[0] != 0x01)
            return false;

        var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHash, 1);
        var iterCount = (int)ReadNetworkByteOrder(decodedHash, 5);
        var saltLength = (int)ReadNetworkByteOrder(decodedHash, 9);

        if (saltLength != _hashOptions.PasswordSaltLength)
            return false;

        if (iterCount < 1)
            return false;

        var salt = new byte[saltLength];
        Buffer.BlockCopy(decodedHash, 13, salt, 0, salt.Length);

        var expectedSubkey = new byte[decodedHash.Length - 13 - salt.Length];
        Buffer.BlockCopy(decodedHash, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

        var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, expectedSubkey.Length);

        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }
    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)buffer[offset + 0] << 24)
               | ((uint)buffer[offset + 1] << 16)
               | ((uint)buffer[offset + 2] << 8)
               | buffer[offset + 3];
    }
    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
}
