using ECC.DanceCup.Auth.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Auth.Domain;

public static class Registrar
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IUserFactory, UserFactory>();
        
        return services;
    }
}