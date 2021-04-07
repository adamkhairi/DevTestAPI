using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth_API.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth_API.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);

    }

    //public class AddRoleModel
    //{
    //}
}
