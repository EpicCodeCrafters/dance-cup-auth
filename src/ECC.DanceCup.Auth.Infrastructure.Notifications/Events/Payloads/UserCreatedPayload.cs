namespace ECC.DanceCup.Auth.Infrastructure.Notifications.Events.Payloads;

public record UserCreatedPayload(
    long UserId,
    string Username
)
{
    public const string EventType = "UserCreated";
}