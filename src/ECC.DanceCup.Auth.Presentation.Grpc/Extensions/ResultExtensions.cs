using ECC.DanceCup.Auth.Application.Errors;
using ECC.DanceCup.Auth.Utils.Extensions;
using FluentResults;
using Grpc.Core;

namespace ECC.DanceCup.Auth.Presentation.Grpc.Extensions;

internal static class ResultExtensions
{
    public static void HandleErrors<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (result.HasError<UnauthorizedError>())
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, result.StringifyErrors()));
        }

        if (result.HasError<NotFoundError>())
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.StringifyErrors()));
        }
        
        if (result.HasError<ApplicationError>())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.StringifyErrors()));
        }
        
        if (result.IsFailed)
        {
            throw new RpcException(new Status(StatusCode.Unknown, result.StringifyErrors()));
        }
    }
}