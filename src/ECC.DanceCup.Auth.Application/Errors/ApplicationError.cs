using FluentResults;

namespace ECC.DanceCup.Auth.Application.Errors;

public abstract class ApplicationError : Error
{
    public ApplicationError(string message)
        : base(message)
    {
    }
}