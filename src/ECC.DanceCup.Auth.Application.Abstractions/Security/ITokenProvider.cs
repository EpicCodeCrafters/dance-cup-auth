using ECC.DanceCup.Auth.Domain.Model;

namespace ECC.DanceCup.Auth.Application.Abstractions.Security;

public interface ITokenProvider
{
    string CreateUserToken(User user);
}