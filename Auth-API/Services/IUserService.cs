using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Models;

namespace Auth_API.Services
{
    public interface IUserService
    {
        Task<List<RegisterModel>> GetUsersAsync();
        Task<RegisterModel> GetUserAsync(string userId);
        //Task<AuthModel> PostUserAsync(RegisterModel newUser);
        Task<RegisterModel> PutUserAsync(string userId, EditUserModel updatedUser);
        Task<bool> DeleteUserAsync(string userId);

        // Roles
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<List<object>> GetRolesList();
        Task<List<object>> GetUsersList();

    }
}