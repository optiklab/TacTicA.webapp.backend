using System;
using System.Threading.Tasks;
using TacTicA.Common.Exceptions;
using TacTicA.Services.Items.Domain.Models;
using TacTicA.Services.Items.Domain.Repositories;

namespace TacTicA.Services.Items.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ItemService(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Guid id, Guid userId, string categoryName, string name, string description, DateTime createdAt)
        {
            var itemCategory = await _categoryRepository.GetAsync(categoryName);
            if (itemCategory == null)
            {
                throw new ActioException("category_not_found", $"Category: '{categoryName}' was not found.");
            }
            await _itemRepository.AddAsync(
                new Item(id, itemCategory.Name, userId, name, description, createdAt)
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            await _itemRepository.DeleteAsync(id);
        }
    }
}