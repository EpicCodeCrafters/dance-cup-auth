using FluentResults;
using MediatR;

namespace ECC.DanceCup.Auth.Application.UseCases.CreateUser;

public static partial class CreateUserUseCase
{
    public record Command(
        string Username,
        string Password
    ) : IRequest<Result<CommandResponse>>;

    public record CommandResponse(
        long UserId,
        string Token
    );
}