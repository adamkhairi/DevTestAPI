using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth_API.Models;
using AutoMapper;

namespace Auth_API.Helpers
{
    public class AppUserProfile :Profile
    {
        public AppUserProfile()
        {
            this.CreateMap<RegisterModel,ApplicationUser >();
            this.CreateMap<ApplicationUser,RegisterModel >();
            this.CreateMap<ApplicationUser,AuthModel>();
            this.CreateMap<ApplicationUser,ApplicationUser>();
            //  this.CreateMap<List<ApplicationUser>,List<RegisterModel>>();

        }
    }
}
