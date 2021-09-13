using System.Collections.Generic;
using System.Threading.Tasks;
using UserCrudApiChallenge.Domain.Entity;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudApiChallenge.Infraestructure.Interface;

namespace UserCrudApiChallenge.Domain.Core
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserRepository _userRepository;

        public UserDomain(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> AddUserAsync(User user) {

            var result = await _userRepository.AddUserAsync(user);

            return user;
        }
        public async Task<bool> UpdateUserAsync(User user) {
            var result = await _userRepository.UpdateUserAsync(user);
            return result;
        }
        public async Task<User> FindUserByIdAsync(string userId) {
            User result = await _userRepository.FindUserByIdAsync(userId);
            return result;
        }

        public async Task<bool> ValidateUserLogin(string email, string password)
        {
            bool result = await _userRepository.ValidateUserLogin(email, password);
            return result;
        }
        public async Task<bool> DeleteUserAsync(string id) {
            bool result = await _userRepository.DeleteUserAsync(id);
            return result;
        }
        public async Task<User> FindUserById(string id)
        {
            User result = await _userRepository.FindUserById(id);
            return result;
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> result = await _userRepository.GetUsers();
            return result;

        }

    }
}