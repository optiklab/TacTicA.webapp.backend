namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class AuthenticateUserRejectedNotification : IRejectedNotificationEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public AuthenticateUserRejectedNotification(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}