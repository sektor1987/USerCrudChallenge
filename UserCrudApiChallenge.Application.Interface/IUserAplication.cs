using System;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.DTO;
using UserCrudApiChallenge.Domain.Entity;

namespace UserCrudApiChallenge.Application.Interface
{
  
        public interface IUserAplication
        {
            Task<UserDTO> AddUserAsync(UserDTO userDto);
            Task<bool> UpdateUserAsync(UserDTO user);
            Task<User> FindUserByIdAsync(string userId);
            Task<bool> DeleteUserAsync(string username);
            Task<User> FindUserByUserName(string username);
        }
    
}
