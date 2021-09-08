using System.Threading.Tasks;
using System.Collections.Generic;
using UserCrudApiChallenge.Domain.Entity;
//using GrpcGvdb;
//using Kapsch.Gvdb.Application.Entity;

namespace UserCrudApiChallenge.Domain.Interface
{
    public interface IUserDomain
    {
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<User> FindUserByIdAsync(string userId);
        Task<bool> DeleteUserAsync(string username);
        Task<User> FindUserByUserName(string username);
    }
}
