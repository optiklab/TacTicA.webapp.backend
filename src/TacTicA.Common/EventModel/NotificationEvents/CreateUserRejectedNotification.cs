namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class CreateUserRejectedNotification : IRejectedNotificationEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreateUserRejectedNotification(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}