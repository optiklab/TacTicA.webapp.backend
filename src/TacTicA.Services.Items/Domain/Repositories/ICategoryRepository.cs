using System.Collections.Generic;
using System.Threading.Tasks;
using TacTicA.Services.Items.Domain.Models;

namespace TacTicA.Services.Items.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetAsync(string name);
        Task<IEnumerable<Category>> BrowseAsync();
        Task AddAsync(Category category);
    }
}