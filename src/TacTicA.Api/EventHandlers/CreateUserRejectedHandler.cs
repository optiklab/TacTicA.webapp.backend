using System;
using System.Threading.Tasks;
using MassTransit;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;

namespace TacTicA.ApiGateway.EventHandlers
{
    public class CreateUserRejectedHandler : IEventHandler<CreateUserRejectedNotification>
    {
        public async Task Consume(ConsumeContext<CreateUserRejectedNotification> context)
        {
            await HandleAsync(context.Message);
        }
        
        private async Task HandleAsync(CreateUserRejectedNotification @event)
        {
            Console.WriteLine($"User creation for {@event.Email} was rejected due to: {@event.Reason}. code: {@event.Code}");

            await Task.CompletedTask;
        }
    }
}