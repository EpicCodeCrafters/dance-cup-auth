using FluentMigrator.Runner;

namespace ECC.DanceCup.Auth.Extensions;

public static class WebHostExtensions
{
    public static async Task MigrateAsync(this IWebHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    
        migrationRunner.MigrateUp();
    }
    
    public static async Task MigrateDryRunAsync(this IWebHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    
        migrationRunner.ListMigrations();
    }
    
    public static async Task MigrateDownAsync(this IWebHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    
        migrationRunner.MigrateDown(0);
    }
}