using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.EventModel.NotificationEvents;
using TacTicA.Common.Exceptions;
using TacTicA.Common.QueueServices;
using TacTicA.Services.Items.Services;

namespace TacTicA.Services.Items.EventHandlers
{
    public class CreateItemHandler : IEventHandler<CreateItemCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IItemService _itemService;
        private ILogger<CreateItemHandler> _logger;

        public CreateItemHandler(IEventBus eventBus, IItemService itemService, ILogger<CreateItemHandler> logger)
        {
            _eventBus = eventBus;
            _itemService = itemService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateItemCommand> context)
        {
            await HandleAsync(context.Message);
        }

        private async Task HandleAsync(CreateItemCommand command)
        {
            _logger.LogInformation($"Creating Item: {@command.Name}");

            try
            {
                await _itemService.AddAsync(command.Id, command.UserId, command.Category,
                    command.Name, command.Description, command.CreatedAt);
                // Fire event by putting event into the queue.
                await _eventBus.PublishAsync(new ItemCreatedNotification(command.Id, command.UserId, command.Category,
                    command.Name, command.Description));
            }
            catch (ActioException ex)
            {
                _logger.LogError(ex.Message);
                await _eventBus.PublishAsync(new CreateItemRejectedNotification(command.UserId, command.Category, command.Name, ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _eventBus.PublishAsync(new CreateItemRejectedNotification(command.UserId, command.Category, command.Name, "Unexpected error", ex.Message));
            }
        }
    }
}