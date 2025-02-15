using AutoFixture;
using AutoFixture.Xunit2;
using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Application.Errors;
using ECC.DanceCup.Auth.Application.UseCases.GetUserToken;
using ECC.DanceCup.Auth.Domain.Model;
using ECC.DanceCup.Auth.Tests.Common.Attributes;
using ECC.DanceCup.Auth.Tests.Common.Errors;
using ECC.DanceCup.Auth.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Auth.Application.Tests.UseCases;

public class GetUserTokenTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        string token,
        string password,
        byte[] passwordHash,
        byte[] passwordSalt,
        [Frozen] Mock<IUserRepository> userRepository,
        [Frozen] Mock<IEncoder> encoder,
        [Frozen] Mock<ITokenProvider> tokenProvider,
        GetUserTokenUseCase.QueryHandler handler,
        IFixture fixture
    )
    {
        //Arrange

        var user = fixture.CreateUser(
            id: 1,
            version: 1,
            createdAt: DateTime.UtcNow,
            changedAt: DateTime.UtcNow,
            username: "Bob",
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );

        userRepository
            .Setup(repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        encoder
            .Setup(coder => coder.CalculateHash(password, user.PasswordSalt))
            .Returns(user.PasswordHash);

        tokenProvider
            .Setup(provider => provider.CreateUserToken(user))
            .Returns(token);

        var query = new GetUserTokenUseCase.Query(user.Username, password);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldBeSuccess();
        result.Value.Token.Should().Be(token);

        userRepository.Verify(
            repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepository.VerifyNoOtherCalls();

        encoder.Verify(
            coder => coder.CalculateHash(password, passwordSalt),
            Times.Once
        );
        encoder.VerifyNoOtherCalls();

        tokenProvider.Verify(
            provider => provider.CreateUserToken(user),
            Times.Once
        );
        tokenProvider.VerifyNoOtherCalls();
    }


    [Theory, AutoMoqData]
    public async Task Handle_UserNotFound_ShouldFail(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt,
        IFixture fixture,
        [Frozen] Mock<IUserRepository> userRepository,
        [Frozen] Mock<IEncoder> encoder,
        [Frozen] Mock<ITokenProvider> tokenProvider,
        GetUserTokenUseCase.QueryHandler handler
    )
    {
        //Arrange

        var user = fixture.CreateUser(
            id: 1,
            version: 1,
            createdAt: DateTime.UtcNow,
            changedAt: DateTime.UtcNow,
            username: "Bob",
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );
        
        userRepository
            .Setup(repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var query = new GetUserTokenUseCase.Query(user.Username, password);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldBeFailWith<UserNotFoundError>();

        userRepository.Verify(
            repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepository.VerifyNoOtherCalls();

        encoder.VerifyNoOtherCalls();
        tokenProvider.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task Handle_SequenceNotEqual_ShouldFail(
        string password,
        byte[] passwordSalt,
        byte[] passwordHash,
        IFixture fixture,
        [Frozen] Mock<IUserRepository> userRepository,
        [Frozen] Mock<IEncoder> encoder,
        [Frozen] Mock<ITokenProvider> tokenProvider,
        GetUserTokenUseCase.QueryHandler handler
    )
    {
        //Arrange
        var user = fixture.CreateUser(
            id: 1,
            version: 1,
            createdAt: DateTime.UtcNow,
            changedAt: DateTime.UtcNow,
            username: "Bob",
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );
        
        userRepository
            .Setup(repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        encoder
            .Setup(coder => coder.CalculateHash(password, user.PasswordSalt))
            .Returns([0]);

        var query = new GetUserTokenUseCase.Query(user.Username, password);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldBeFailWith<UnauthorizedError>();

        userRepository.Verify(
            repository => repository.FindAsync(user.Username, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepository.VerifyNoOtherCalls();

        encoder.Verify(
            coder => coder.CalculateHash(password, user.PasswordSalt),
            Times.Once
        );
        encoder.VerifyNoOtherCalls();

        tokenProvider.VerifyNoOtherCalls();
    }
}