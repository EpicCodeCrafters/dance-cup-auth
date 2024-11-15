namespace ECC.DanceCup.Auth.Application.Errors;

public class UserAlreadyExistsError : ApplicationError
{
    public UserAlreadyExistsError(string username)
        : base($"Пользователь с логином {username} уже существует")
    {
    }
}