using System.Collections.Generic;
using System.Threading.Tasks;
using TacTicA.Services.Items.Domain.Models;
using TacTicA.Services.Items.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TacTicA.Services.Items.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDatabase _database;

        public CategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Category category)
            => await Collection.InsertOneAsync(category);

        public async Task<IEnumerable<Category>> BrowseAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task<Category> GetAsync(string name)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Name == name.ToLowerInvariant()); // <= MongoDB.Driver.Linq

        private IMongoCollection<Category> Collection
            => _database.GetCollection<Category>("Categories");
    }
}