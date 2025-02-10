using ECC.DanceCup.Auth.Application.Abstractions.Storage;
using ECC.DanceCup.Auth.Infrastructure.Storage.Options;
using ECC.DanceCup.Auth.Infrastructure.Storage.Postgres;
using ECC.DanceCup.Auth.Infrastructure.Storage.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Auth.Infrastructure.Storage;

public static class Registrar
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddTransient<IPostgresConnectionFactory, PostgresConnectionFactory>();

        services.Configure<StorageOptions>(configuration.GetSection("StorageOptions"));

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var connectionString = configuration["StorageOptions:ConnectionString"];
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder =>
            {
                runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Registrar).Assembly);
            });

        return services;
    }

}