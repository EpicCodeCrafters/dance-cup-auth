using ECC.DanceCup.Auth.Domain.Model;

namespace ECC.DanceCup.Auth.Application.Abstractions.Storage;

public interface IUserRepository
{
    Task<long> InsertAsync(User user, CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);

    Task<User?> FindAsync(string username, CancellationToken cancellationToken);
}