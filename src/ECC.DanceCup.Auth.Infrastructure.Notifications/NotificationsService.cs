using System.Text.Json;
using Confluent.Kafka;
using ECC.DanceCup.Auth.Application.Abstractions.Notifications;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Events;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Events.Payloads;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Options;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Auth.Infrastructure.Notifications;

public class NotificationsService : INotificationsService, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly IOptions<KafkaOptions> _kafkaOptions;
    private readonly ILogger<NotificationsService> _logger;

    public NotificationsService(
        IProducerProvider producerProvider, 
        IOptions<KafkaOptions> kafkaOptions,
        ILogger<NotificationsService> logger)
    {
        _producer = producerProvider.Create<string, string>();
        _kafkaOptions = kafkaOptions;
        _logger = logger;
    }

    public Task NotifyUserCreatedAsync(long userId, string username, CancellationToken cancellationToken)
    {
        var payload = new UserCreatedPayload(userId, username);
        var danceCupEvent = new DanceCupEvent(
            Type: UserCreatedPayload.EventType,
            Payload: JsonSerializer.Serialize(payload)
        );

        try
        {
            _producer.Produce(
                _kafkaOptions.Value.Topics.DanceCupEvents.Name,
                new Message<string, string>
                {
                    Key = userId.ToString(),
                    Value = JsonSerializer.Serialize(danceCupEvent)
                }
            );
        }
        catch
        {
            _logger.LogError("Не удалось отправить сообщение о том, что пользователь {UserId} создан", userId);
        } 
        
        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}