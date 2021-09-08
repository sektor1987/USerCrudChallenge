using System;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Domain.Entity;
using UserCrudApiChallenge.Domain;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudApiChallenge.Application.DTO;

namespace UserCrudApiChallenge.Application.Main
{
    public class UserApplication : IUserAplication
    {
        private readonly IUserDomain _userDomain;

        public UserApplication(IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto) {

            User user = new();
            Guid guid = new();
            user.Id = guid.ToString();
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            var user_ = await _userDomain.AddUserAsync(user);
            return userDto;

        }
        public async Task<bool> UpdateUserAsync(UserDTO userDto) {
            User user = new();
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            return await _userDomain.UpdateUserAsync(user);
            
        }
        public async Task<User> FindUserByIdAsync(string userId) {
            return await _userDomain.FindUserByIdAsync(userId);

        }
        public async Task<bool> DeleteUserAsync(string username) {
            return await _userDomain.DeleteUserAsync(username);

        }
        public async Task<User> FindUserByUserName(string username) {
            return await _userDomain.FindUserByUserName(username);

        }
    }
}
