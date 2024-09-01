using System;
using System.Threading.Tasks;
using MassTransit;
using TacTicA.ApiGateway.Models;
using TacTicA.ApiGateway.Repositories;
using TacTicA.Common;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;

namespace TacTicA.ApiGateway.EventHandlers;

public class ItemCreatedHandler : IEventHandler<ItemCreatedNotification>
{
    private readonly IFlattenedItemRepository _itemRepository;

    public ItemCreatedHandler(IFlattenedItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task Consume(ConsumeContext<ItemCreatedNotification> context)
    {
        await HandleAsync(context.Message);
    }
    
    private async Task HandleAsync(ItemCreatedNotification @event)
    {
        await _itemRepository.AddAsync(new FlattenedItem
        {
            // See https://martinfowler.com/eaaDev/EventNarrative.html
            // With event collaboration every component stores all the data it needs and listens to update events for that data.

            // Flattened DTO model. It is used to do not pass whole object to Message Queue, but instead
            // save object in the microservice database.
            // In this case, upon Get API call it can query object from internal database.
            // Or we can use this information to execute scheduled task of sending emails or fraud preventions mechanisms.
            Id = @event.Id,
            Name = @event.Name,
            Category = @event.Category,
            Description = @event.Description,
            UserId = @event.UserId,
            CreatedAt = @event.CreatedAt
        });
        Console.WriteLine($"Item created: {@event.Name}");
    }
}