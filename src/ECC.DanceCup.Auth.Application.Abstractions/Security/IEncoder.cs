namespace ECC.DanceCup.Auth.Application.Abstractions.Security;

public interface IEncoder
{
    (byte[] hash, byte[] salt) CalculateHash(string value);

    byte[] CalculateHash(string value, byte[] salt);
}