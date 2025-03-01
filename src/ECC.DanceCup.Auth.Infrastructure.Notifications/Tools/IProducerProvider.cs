using Confluent.Kafka;

namespace ECC.DanceCup.Auth.Infrastructure.Notifications.Tools;

public interface IProducerProvider
{
    IProducer<TKey, TValue> Create<TKey, TValue>();
}