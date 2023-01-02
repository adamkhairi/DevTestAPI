using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Models;

namespace Auth_API.Services.Users
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        Task<List<UserModel>> GetAll();
        Task<UserModel> Get(string userId);
        Task<UserModel> Put(EditUserModel updatedUser);
        Task<bool> Delete(string userId);

        // Roles
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<List<object>> GetRolesList();
        Task<List<object>> GetUsersList();
    }
}