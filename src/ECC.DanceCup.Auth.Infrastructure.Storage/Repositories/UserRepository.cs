using System.Data;
using Dapper;
using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Domain.Model;
using ECC.DanceCup.Auth.Infrastructure.Storage.Postgres;

namespace ECC.DanceCup.Auth.Infrastructure.Storage.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public UserRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> InsertAsync(User user, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            insert into "users" ("version", "created_at", "changed_at", "username", "password_hash", "password_salt")
            values (@Version, @CreatedAt, @ChangedAt, @Username, @PasswordHash, @PasswordSalt)
            returning "id";
            """;

        var userId = await connection.QuerySingleAsync<long>(sqlCommand, user);

        return userId;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            update "users"
               set "version" = @Version + 1
                 , "created_at" = @CreatedAt
                 , "changed_at" = @ChangedAt
                 , "username" = @Username
                 , "password_hash" = @PasswordHash
                 , "password_salt" = @PasswordSalt
             where "id" = @Id
               and "version" = @Version;
            """;

        var updatedCount = await connection.ExecuteAsync(sqlCommand, user);
        if (updatedCount == 0)
        {
            throw new DBConcurrencyException("Не удалось обновить пользователя");
        }
    }

    public async Task<User?> FindAsync(string username, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            select u."id" as id
                 , u."version" as version
                 , u.created_at as created_at
                 , u."changed_at" as changed_at
                 , u."username" as username
                 , u."password_hash" as password_hash
                 , u."password_salt" as password_salt
              from "users" as u
             where u."username" = @Username;
            """;

        return await connection.QuerySingleOrDefaultAsync<User>(
            sqlCommand,
            new
            {
                Username = username
            }
        );
    }
}