using Npgsql;

namespace ECC.DanceCup.Auth.Infrastructure.Storage.Postgres;

public interface IPostgresConnectionFactory
{
    Task<NpgsqlConnection> CreateAsync();
}