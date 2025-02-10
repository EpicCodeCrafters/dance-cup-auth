using ECC.DanceCup.Auth;
using ECC.DanceCup.Auth.Extensions;
using Microsoft.AspNetCore;

var host = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddEnvironmentVariables();
    })
    .Build();

switch (args)
{
    case ["--migrate"]:
        await host.MigrateAsync();
        return;
    
    case ["--migrate", "dry-run"]:
        await host.MigrateDryRunAsync();
        return;
    
    case ["--migrate", "down"]:
        await host.MigrateDownAsync();
        return;
    
    default:
        await host.RunAsync();
        return;
}