using System;
using System.Threading.Tasks;
using MassTransit;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;

namespace TacTicA.ApiGateway.EventHandlers
{
    public class UserAuthenticatedHandler : IEventHandler<UserAuthenticatedNotification>
    {
        public async Task Consume(ConsumeContext<UserAuthenticatedNotification> context)
        {
            await HandleAsync(context.Message);
        }
        
        private async Task HandleAsync(UserAuthenticatedNotification @event)
        {
            Console.WriteLine($"User authenticated: {@event.Email}");

            await Task.CompletedTask;
        }
    }
}