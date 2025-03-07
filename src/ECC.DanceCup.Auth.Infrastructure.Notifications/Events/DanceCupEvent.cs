namespace ECC.DanceCup.Auth.Infrastructure.Notifications.Events;

public record DanceCupEvent(
    string Type,
    string Payload
);