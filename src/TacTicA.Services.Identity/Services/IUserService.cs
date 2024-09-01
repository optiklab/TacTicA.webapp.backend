using System;
using System.Threading.Tasks;
using TacTicA.Common.Auth;

namespace TacTicA.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string name, string email, string password);
        Task<JsonWebToken> LoginAsync(string email, string password);
        Task<JsonCurrentUser> GetCurrentUserInfoAsync(Guid id);
    }
}