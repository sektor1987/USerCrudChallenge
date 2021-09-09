using System;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Domain.Entity;
using UserCrudApiChallenge.Domain;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudApiChallenge.Application.DTO;
using System.Collections.Generic;

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
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            User user_ = await _userDomain.AddUserAsync(user);
            return userDto;

        }
        public async Task<bool> UpdateUserAsync(UserDTO userDto) {
            User user = new();
            user.Name = userDto.Name;
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

        public async Task<List<UserDTO>> GetUsers()
        {
            List<UserDTO> lstUserDto = new();
#warning falta un mapeo mas elegante entre el DTO y la Entity, por falta de tiempo lo dejamos así
            Task<List<User>> lstUsers = _userDomain.GetUsers();

            foreach (User user in await lstUsers)
            {
                UserDTO userDTO = new();
                userDTO.Name = user.Name;
                userDTO.Password = user.Password;
                userDTO.Email = user.Email;
                lstUserDto.Add(userDTO);
            }

            return lstUserDto;
        }

    }
}
