namespace ECC.DanceCup.Auth.Application.Abstractions.Notifications;

public interface INotificationsService
{
    Task NotifyUserCreatedAsync(long userId, string username, CancellationToken cancellationToken);
}