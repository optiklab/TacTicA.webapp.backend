namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class UserAuthenticatedNotification : INotificationEvent
    {
        protected UserAuthenticatedNotification()
        {
        }

        public UserAuthenticatedNotification(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}