using System;
using System.Threading.Tasks;
using TacTicA.Services.Items.Domain.Models;
using TacTicA.Services.Items.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TacTicA.Services.Items.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoDatabase _database;

        public ItemRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Item item)
            => await Collection.InsertOneAsync(item);

        public async Task<Item> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id); // <= MongoDB.Driver.Linq

        public async Task DeleteAsync(Guid id)
            => await Collection.DeleteOneAsync(x => x.Id == id);

        private IMongoCollection<Item> Collection
            => _database.GetCollection<Item>("Items");
    }
}