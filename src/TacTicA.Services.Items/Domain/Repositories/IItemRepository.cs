using System;
using System.Threading.Tasks;
using TacTicA.Services.Items.Domain.Models;

namespace TacTicA.Services.Items.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item> GetAsync(Guid id);
        Task AddAsync(Item item);
        Task DeleteAsync(Guid id);
    }
}