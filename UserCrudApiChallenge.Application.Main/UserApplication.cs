using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.DTO;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Domain.Entity;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudChallenge.CrossCutting.Common;

namespace UserCrudApiChallenge.Application.Main
{
    public class UserApplication : IUserAplication
    {
        private readonly IUserDomain _userDomain;
        private IManagerEncryptDecrypt _managerEncryptDecrypt;

        public UserApplication(IUserDomain userDomain, IManagerEncryptDecrypt managerEncryptDecrypt)
        {
            _userDomain = userDomain;
            _managerEncryptDecrypt = managerEncryptDecrypt;
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto)
        {
            User user = new User();
            Guid guid = Guid.NewGuid();
            user.Id = guid.ToString();
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = _managerEncryptDecrypt.Encrypt(userDto.Password);
            User user_ = await _userDomain.AddUserAsync(user);
            userDto.Id = user.Id;
            return userDto;
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDto)
        {
            User user = new User();
            user.Id = userDto.Id;
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = _managerEncryptDecrypt.Encrypt(userDto.Password);
            return await _userDomain.UpdateUserAsync(user);
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            return await _userDomain.FindUserByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            return await _userDomain.DeleteUserAsync(id);
        }

        public async Task<UserDTO> FindUserById(string id)
        {
#warning falta validar si es null
            Task<User> userEntity = _userDomain.FindUserById(id);
            UserDTO userDTO = new UserDTO();
            userDTO.Id = userEntity.Result.Id;
            userDTO.Name = userEntity.Result.Name;
            userDTO.Password = _managerEncryptDecrypt.Decrypt(userEntity.Result.Password);
            userDTO.Email = userEntity.Result.Email;
            return userDTO;
        }

        public async Task<bool> ValidateUserLogin(string email, string password)
        {
#warning falta validar si es null
            Task<bool> validateUser = _userDomain.ValidateUserLogin(email, _managerEncryptDecrypt.Encrypt(password));
            return await validateUser;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            List<UserDTO> lstUserDto = new List<UserDTO>();
#warning falta un mapeo mas elegante entre el DTO y la Entity, por falta de tiempo lo dejamos así
            Task<List<User>> lstUsers = _userDomain.GetUsers();

            foreach (User user in await lstUsers)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Name = user.Name;
                userDTO.Id = user.Id;
                userDTO.Password = _managerEncryptDecrypt.Decrypt(user.Password);
                userDTO.Email = user.Email;
                lstUserDto.Add(userDTO);
            }

            return lstUserDto;
        }
    }
}