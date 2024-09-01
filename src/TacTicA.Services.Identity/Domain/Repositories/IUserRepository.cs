using System;
using System.Threading.Tasks;
using TacTicA.Services.Identity.Domain.Models;

namespace TacTicA.Services.Identity.Domain.Repositories
{
    public interface IUserRepository
    {
         Task<User> GetAsync(Guid id);
         Task<User> GetAsync(string email);
         Task AddAsync(User user);
    }
}