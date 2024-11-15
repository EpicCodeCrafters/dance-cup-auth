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

if (args is ["--migrate"])
{
    await host.MigrateAsync();
    return;
}

await host.RunAsync();