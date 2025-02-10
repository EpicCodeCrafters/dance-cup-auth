using ECC.DanceCup.Auth.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Auth.Domain.Services;

public interface IUserFactory
{
    Result<User> Create(string username, byte[] passwordHash, byte[] passwordSalt);
}