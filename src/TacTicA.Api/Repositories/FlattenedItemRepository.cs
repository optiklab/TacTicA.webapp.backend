using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TacTicA.ApiGateway.Models;

namespace TacTicA.ApiGateway.Repositories
{
    public class FlattenedItemRepository : IFlattenedItemRepository
    {
        private readonly IMongoDatabase _database;

        public FlattenedItemRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(FlattenedItem item)
            => await Collection.InsertOneAsync(item);

        public async Task<IEnumerable<FlattenedItem>> BrowseAsync(Guid userId)
            => await Collection
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();

        public async Task<FlattenedItem> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id); // <= MongoDB.Driver.Linq

        private IMongoCollection<FlattenedItem> Collection
            => _database.GetCollection<FlattenedItem>("Items");
    }
}