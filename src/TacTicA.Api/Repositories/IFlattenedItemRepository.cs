using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TacTicA.ApiGateway.Models;

namespace TacTicA.ApiGateway.Repositories
{
    public interface IFlattenedItemRepository
    {
        Task<FlattenedItem> GetAsync(Guid id);
        Task AddAsync(FlattenedItem item);
        Task<IEnumerable<FlattenedItem>> BrowseAsync(Guid userId);
    }
}