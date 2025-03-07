using Confluent.Kafka;
using ECC.DanceCup.Auth.Infrastructure.Notifications.Options;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Auth.Infrastructure.Notifications.Tools;

public class ProducerProvider : IProducerProvider
{
    private readonly IOptions<KafkaOptions> _kafkaOptions;

    public ProducerProvider(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions;
    }

    public IProducer<TKey, TValue> Create<TKey, TValue>()
    {

        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaOptions.Value.BootstrapServers
        };
        
        return new ProducerBuilder<TKey, TValue>(config).Build();
    }
}