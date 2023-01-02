using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Models;

namespace Auth_API.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<List<object>> GetRolesList();
        Task<List<object>> GetUsersList();
    }

    //public class AddRoleModel
    //{
    //}
}