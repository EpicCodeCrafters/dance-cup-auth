using ECC.DanceCup.Auth.Application.Abstractions.Notifications;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Options;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Auth.Infrastructure.Notifications;

public static class Registrar
{
    public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INotificationsService, NotificationsService>();

        services.AddScoped<IProducerProvider, ProducerProvider>();

        services.Configure<KafkaOptions>(configuration.GetSection("KafkaOptions"));
        
        return services;
    }
}