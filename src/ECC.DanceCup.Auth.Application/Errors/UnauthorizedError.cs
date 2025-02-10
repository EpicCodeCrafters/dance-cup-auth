namespace ECC.DanceCup.Auth.Application.Errors;

public class UnauthorizedError : ApplicationError
{
    public UnauthorizedError()
        : base("Недостаточно прав для совершения операции")
    {
    }
}