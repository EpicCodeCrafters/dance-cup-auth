using ECC.DanceCup.Auth.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Auth.Domain.Services;

public class UserFactory : IUserFactory
{
    public Result<User> Create(string username, byte[] passwordHash, byte[] passwordSalt)
    {
        var now = DateTime.UtcNow;

        var user = new User(
            id: default,
            version: 1,
            createdAt: now,
            changedAt: now,
            username: username,
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );

        return user;
    }
}