using System.Security.Cryptography;
using ECC.DanceCup.Auth.Application.Abstractions.Security;

namespace ECC.DanceCup.Auth.Infrastructure.Security.Services;

public class Encoder : IEncoder
{
    public (byte[] hash, byte[] salt) CalculateHash(string value)
    {
        using var hmac = new HMACSHA512();

        var salt = hmac.Key;
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));

        return (hash, salt);
    }

    public byte[] CalculateHash(string value, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
    }
}