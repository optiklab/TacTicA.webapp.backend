namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class CreateItemRejectedNotification : IRejectedNotificationEvent
    {
        public Guid Id { get; }
        public string Category { get; }
        public string Name { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreateItemRejectedNotification(Guid id, string category, string name, string reason, string code)
        {
            Id = id;
            Category = Category;
            Name = Name;
            Reason = reason;
            Code = code;
        }
    }
}