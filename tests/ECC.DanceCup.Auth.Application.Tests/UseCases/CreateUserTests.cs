using AutoFixture.Xunit2;
using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Application.UseCases.CreateUser;
using ECC.DanceCup.Auth.Domain.Model;
using ECC.DanceCup.Auth.Domain.Services;
using ECC.DanceCup.Auth.Tests.Common.Attributes;
using ECC.DanceCup.Auth.Tests.Common.Errors;
using ECC.DanceCup.Auth.Tests.Common.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECC.DanceCup.Auth.Application.Tests.UseCases;

public class CreateUserTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        string userName,
        string password,
        byte[] passwordHash,
        byte[] passwordSalt,
        User user,
        long userId,
        string token,
        [Frozen] Mock<IUserFactory> userFactoryMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IEncoder> userEncoderMock,
        [Frozen] Mock<ITokenProvider> tokenProviderMock,
        CreateUserUseCase.CommandHandler handler
    )
    {
        //Arrange
        
        userFactoryMock
            .Setup(userFactory => userFactory.Create(userName, passwordHash, passwordSalt))
            .Returns(user);

        userRepositoryMock
            .Setup(userRepository => userRepository.InsertAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);
        
        userRepositoryMock
            .Setup(userRepository => userRepository.FindAsync(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        
        userEncoderMock
            .Setup(userEncoder => userEncoder.CalculateHash(password))
            .Returns((passwordHash, passwordSalt));
        
        tokenProviderMock
            .Setup(tokenProvider => tokenProvider.CreateUserToken(user))
            .Returns(token);
        
        var command = new CreateUserUseCase.Command(userName, password);
        
        //Act
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        
        result.ShouldBeSuccess();
        result.Value.UserId.Should().Be(userId);
        result.Value.Token.Should().Be(token);
        
        userFactoryMock.Verify(
            userFactory => userFactory.Create(userName, passwordHash, passwordSalt),
            Times.Once
        );
        userFactoryMock.VerifyNoOtherCalls();
        
        userRepositoryMock.Verify(
            userRepository => userRepository.InsertAsync(user, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepositoryMock.Verify(
            userRepository => userRepository.FindAsync(userName, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepositoryMock.VerifyNoOtherCalls();
        
        userEncoderMock.Verify(
            userEncoder => userEncoder.CalculateHash(password),
            Times.Once
        );
        userEncoderMock.VerifyNoOtherCalls();
        
        tokenProviderMock.Verify(
            tokenProvider => tokenProvider.CreateUserToken(user),
            Times.Once
        );
        tokenProviderMock.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task Handle_UserFactoryCreateFailed_ShouldFail(
        string userName,
        string password,
        byte[] passwordHash,
        byte[] passwordSalt,
        [Frozen] Mock<IUserFactory> userFactoryMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IEncoder> userEncoderMock,
        [Frozen] Mock<ITokenProvider> tokenProviderMock,
        CreateUserUseCase.CommandHandler handler
    )
    {
        //Arrange
        
        userFactoryMock
            .Setup(userFactory => userFactory.Create(userName, passwordHash, passwordSalt))
            .Returns(new TestError());
        
        userRepositoryMock
            .Setup(userRepository => userRepository.FindAsync(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        
        userEncoderMock
            .Setup(userEncoder => userEncoder.CalculateHash(password))
            .Returns((passwordHash, passwordSalt));
        
        var command = new CreateUserUseCase.Command(userName, password);
        
        //Act
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        
        result.ShouldBeFailWith<TestError>();
        
        userFactoryMock.Verify(
            userFactory => userFactory.Create(userName, passwordHash, passwordSalt),
            Times.Once
        );
        userFactoryMock.VerifyNoOtherCalls();
        
        userRepositoryMock.Verify(
            userRepository => userRepository.FindAsync(userName, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepositoryMock.VerifyNoOtherCalls();
        
        userEncoderMock.Verify(
            userEncoder => userEncoder.CalculateHash(password),
            Times.Once
        );
        userEncoderMock.VerifyNoOtherCalls();
        
        tokenProviderMock.VerifyNoOtherCalls();
    }
}