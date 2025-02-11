using AutoFixture;
using ECC.DanceCup.Auth.Domain.Model;

namespace ECC.DanceCup.Auth.Domain.Tests.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<User>(composer =>
            composer.FromFactory(() => new User(
                id: fixture.Create<long>(),
                version: fixture.Create<int>(),
                createdAt: fixture.Create<DateTime>(),
                changedAt: fixture.Create<DateTime>(),
                username: fixture.Create<string>(),
                passwordHash: fixture.Create<byte[]>(),
                passwordSalt: fixture.Create<byte[]>()
            ))
        );
    }
}