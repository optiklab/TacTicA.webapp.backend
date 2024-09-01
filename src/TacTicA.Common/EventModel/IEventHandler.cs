using MassTransit;

namespace TacTicA.Common.EventModel
{
    public interface IEventHandler<in T> : IConsumer<T> where T : class, IEvent
    {
    }
}