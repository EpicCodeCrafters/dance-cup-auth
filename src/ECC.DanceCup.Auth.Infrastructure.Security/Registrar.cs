using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Infrastructure.Security.Options;
using ECC.DanceCup.Auth.Infrastructure.Security.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Auth.Infrastructure.Security;

public static class Registrar
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEncoder, Encoder>();

        services.AddScoped<ITokenProvider, TokenProvider>();
        
        services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));

        return services;
    }
}