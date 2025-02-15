using AutoFixture;
using ECC.DanceCup.Auth.Domain.Model;

namespace ECC.DanceCup.Auth.Tests.Common.Extensions;

public static class FixtureExtensions
{
    public static User CreateUser(
        this IFixture fixture,
        long? id = null,
        int? version = null,
        DateTime? createdAt = null,
        DateTime? changedAt = null,
        string? username = null,
        byte[]? passwordHash = null,
        byte[]? passwordSalt = null)
    {
        id ??= fixture.Create<long>();
        version ??= fixture.Create<int>();
        createdAt ??= fixture.Create<DateTime>();
        changedAt ??= fixture.Create<DateTime>();
        username ??= fixture.Create<string>();
        passwordHash ??= fixture.Create<byte[]>();
        passwordSalt ??= fixture.Create<byte[]>();

        return new User(
            id: id.Value,
            version: version.Value,
            createdAt: createdAt.Value,
            changedAt: changedAt.Value,
            username: username,
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );
    }
}