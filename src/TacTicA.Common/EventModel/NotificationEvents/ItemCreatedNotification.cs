namespace TacTicA.Common.EventModel.NotificationEvents
{
    public class ItemCreatedNotification : IAuthenticatedNotificationEvent
    {
        protected ItemCreatedNotification()
        {
            //CreatedAt = DateTime.UtcNow;
        }

        public ItemCreatedNotification(Guid id, Guid userId, string category, string name, string description)
        {
            Id = id;
            UserId = userId;
            Category = category;
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public Guid UserId { get; }
        public string Category { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
    }
}