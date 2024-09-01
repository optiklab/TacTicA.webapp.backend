using MassTransit;

namespace TacTicA.Common.QueueServices;

public class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)  where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
}