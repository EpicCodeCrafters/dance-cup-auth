using FluentResults;
using MediatR;

namespace ECC.DanceCup.Auth.Application.UseCases.GetUserToken;

public static partial class GetUserTokenUseCase
{
    public record Query(
        string Username,
        string Password
    ) : IRequest<Result<QueryResponse>>;

    public record QueryResponse(string Token);
}