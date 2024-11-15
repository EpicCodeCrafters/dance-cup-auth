using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Application.Errors;
using ECC.DanceCup.Auth.Domain.Services;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Auth.Application.UseCases.CreateUser;

public static partial class CreateUserUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result<CommandResponse>>
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IEncoder _encoder;

        public CommandHandler(
            IUserFactory userFactory,
            IUserRepository userRepository,
            IEncoder encoder)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _encoder = encoder;
        }

        public async Task<Result<CommandResponse>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindAsync(command.Username, cancellationToken);
            if (existingUser is not null)
            {
                return new UserAlreadyExistsError(command.Username);
            }

            var (passwordHash, passwordSalt) = _encoder.CalculateHash(command.Password);

            var createUserResult = _userFactory.Create(command.Username, passwordHash, passwordSalt);
            if (createUserResult.IsFailed)
            {
                return createUserResult.ToResult();
            }

            var user = createUserResult.Value;
            var userId = await _userRepository.InsertAsync(user, cancellationToken);

            return new CommandResponse(userId);
        }
    }
}