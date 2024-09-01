using System;
using System.Threading.Tasks;
using MassTransit;
using TacTicA.Common;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;

namespace TacTicA.ApiGateway.EventHandlers
{
    public class UserCreatedHandler : IEventHandler<UserCreatedNotification>
    {
        public async Task Consume(ConsumeContext<UserCreatedNotification> context)
        {
            await HandleAsync(context.Message);
        }
        
        private async Task HandleAsync(UserCreatedNotification @event)
        {
            Console.WriteLine($"User created: {@event.Name} {@event.Email}");

            await Task.CompletedTask;
        }
    }
}