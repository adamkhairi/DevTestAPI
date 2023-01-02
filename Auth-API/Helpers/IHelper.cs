using System.Threading.Tasks;
using Auth_API.Models;

namespace Auth_API.Helpers
{
    public interface IHelper
    {
      public string Check(string oldString, string newString);
      public bool UploadImg(byte[] imgByte, string folder);
    }
}