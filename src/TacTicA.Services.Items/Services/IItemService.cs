using System;
using System.Threading.Tasks;

namespace TacTicA.Services.Items.Services
{
    public interface IItemService
    {
        Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdAt);

        Task DeleteAsync(Guid id);
    }
}