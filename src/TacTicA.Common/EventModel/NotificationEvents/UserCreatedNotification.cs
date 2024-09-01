namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class UserCreatedNotification : INotificationEvent
    {
        protected UserCreatedNotification()
        {
        }

        public UserCreatedNotification(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }
        public string Name { get; }
    }
}