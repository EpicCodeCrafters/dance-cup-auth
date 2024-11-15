using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Application.Errors;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Auth.Application.UseCases.GetUserToken;

public static partial class GetUserTokenUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncoder _encoder;
        private readonly ITokenProvider _tokenProvider;

        public QueryHandler(
            IUserRepository userRepository, 
            IEncoder encoder, 
            ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _encoder = encoder;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<QueryResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(query.Username, cancellationToken);
            if (user is null)
            {
                return new UserNotFoundError(query.Username);
            }

            var passwordHash = _encoder.CalculateHash(query.Password, user.PasswordSalt);
            if (passwordHash.SequenceEqual(user.PasswordHash) is false)
            {
                return new UnauthorizedError();
            }

            var token = _tokenProvider.CreateUserToken(user);

            return new QueryResponse(token);
        }
    }
}