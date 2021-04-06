using System.Threading.Tasks;
using Auth_API.Models;

namespace Auth_API.Helpers
{
    public interface IHelper
    {
         Task<AuthModel> AddUser(RegisterModel model);
    }
}