using System;
using System.Threading.Tasks;
using MassTransit;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;

namespace TacTicA.Services.Items.EventHandlers
{
    public class CreateItemRejectedHandler : IEventHandler<CreateItemRejectedNotification>
    {
        public async Task Consume(ConsumeContext<CreateItemRejectedNotification> context)
        {
            await HandleAsync(context.Message);
        }

        private async Task HandleAsync(CreateItemRejectedNotification @event)
        {
            Console.WriteLine($"Item '{@event.Name}' creation for category '{@event.Category}' was rejected due to: {@event.Reason}. {@event.Code}");

            await Task.CompletedTask;
        }
    }
}