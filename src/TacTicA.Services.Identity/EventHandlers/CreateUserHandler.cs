using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.EventModel.NotificationEvents;
using TacTicA.Common.Exceptions;
using TacTicA.Common.QueueServices;
using TacTicA.Services.Identity.Services;

namespace TacTicA.Services.Identity.EventHandlers
{
    public class CreateUserHandler : IEventHandler<CreateUserCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IUserService _userService;
        private ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IEventBus eventBus, IUserService userService, ILogger<CreateUserHandler> logger)
        {
            _eventBus = eventBus;
            _userService = userService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateUserCommand> context)
        {
            await HandleAsync(context.Message);
        }
        
        private async Task HandleAsync(CreateUserCommand command)
        {
            _logger.LogInformation($"Creating User: {@command.Email} {@command.Name}");

            try
            {
                await _userService.RegisterAsync(command.Name, command.Email, command.Password);
                // Fire event by putting event into the queue.
                await _eventBus.PublishAsync(new UserCreatedNotification(command.Email, command.Name));
            }
            catch (ActioException ex)
            {
                await _eventBus.PublishAsync(new CreateUserRejectedNotification(command.Email, ex.Code, ex.Message));
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                await _eventBus.PublishAsync(new CreateUserRejectedNotification(command.Email, "Unexpected error", ex.Message));
                _logger.LogError(ex.Message);
            }
        }
    }
}