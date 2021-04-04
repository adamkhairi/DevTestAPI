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
            this.CreateMap<ApplicationUser, RegisterModel>();

        }
    }
}
