namespace ECC.DanceCup.Auth.Application.Errors;

public class UserNotFoundError : NotFoundError
{
    public UserNotFoundError(string username)
        : base($"Пользователь с логином {username} не найден")
    {
    }
}