namespace TacTicA.Common.EventModel.NotificationEvents
{
    public interface IAuthenticatedNotificationEvent : INotificationEvent
    {
        Guid UserId { get; }
    }
}