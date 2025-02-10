namespace ECC.DanceCup.Auth.Application.Errors;

public abstract class NotFoundError : ApplicationError
{
    public NotFoundError(string message)
        : base(message)
    {
    }
}