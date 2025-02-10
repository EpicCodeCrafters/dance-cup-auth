using ECC.DanceCup.Auth.Application.UseCases.CreateUser;
using ECC.DanceCup.Auth.Application.UseCases.GetUserToken;
using ECC.DanceCup.Auth.Presentation.Grpc.Extensions;
using Grpc.Core;
using MediatR;

namespace ECC.DanceCup.Auth.Presentation.Grpc;

public class DanceCupAuthGrpcService : DanceCupAuth.DanceCupAuthBase
{
    private readonly ISender _sender;

    public DanceCupAuthGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var command = new CreateUserUseCase.Command(request.Username, request.Password);
        var result = await _sender.Send(command, context.CancellationToken);
        
        result.HandleErrors();

        return new CreateUserResponse
        {
            UserId = result.Value.UserId,
            Token = result.Value.Token
        };
    }

    public override async Task<GetUserTokenResponse> GetUserToken(GetUserTokenRequest request, ServerCallContext context)
    {
        var query = new GetUserTokenUseCase.Query(request.Username, request.Password);
        var result = await _sender.Send(query, context.CancellationToken);
        
        result.HandleErrors();

        return new GetUserTokenResponse
        {
            Token = result.Value.Token
        };
    }
}