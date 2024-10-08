using System;
using System.Threading.Tasks;
using TacTicA.Common.Auth;
using TacTicA.Common.Exceptions;
using TacTicA.Services.Identity.Domain.Models;
using TacTicA.Services.Identity.Domain.Repositories;
using TacTicA.Services.Identity.Domain.Services;

namespace TacTicA.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IEncrypter encrypter, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _jwtHandler = jwtHandler;
        }

        public async Task RegisterAsync(string name, string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ActioException("email_in_user", $"Email: '{email}' is already in use.");
            }
            user = new User(name, email);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials.");
            }
            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials.");
            }

            return _jwtHandler.Create(user.Id);
        }

        public async Task<JsonCurrentUser> GetCurrentUserInfoAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new ActioException("invalid_id", $"Invalid id.");
            }

            return new JsonCurrentUser
            {
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}