namespace TacTicA.Common.EventModel.NotificationEvents
{
    public interface IRejectedNotificationEvent : INotificationEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}