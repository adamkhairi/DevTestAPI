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
            CreateMap<RegisterModel,ApplicationUser >();
            CreateMap<ApplicationUser,RegisterModel >();
            CreateMap<ApplicationUser,AuthModel>();
            CreateMap<ApplicationUser,ApplicationUser>();
            CreateMap<EditUserModel,RegisterModel>();
            CreateMap<EditUserModel,ApplicationUser>()
            .ForMember(dest => dest.Email ,opt=> opt.PreCondition(src => !string.IsNullOrEmpty(src.Email) ))
            .ForMember(dest => dest.FirstName,opt => opt.PreCondition(src => string.IsNullOrEmpty(src.FirstName)))
            .ForMember(dest => dest.LastName,opt=> opt.PreCondition(src => !string.IsNullOrEmpty(src.LastName) ))
            .ForMember(dest => dest.PhoneNumber,opt=> opt.PreCondition(src => !string.IsNullOrEmpty(src.PhoneNumber)));
            //  this.CreateMap<List<ApplicationUser>,List<RegisterModel>>();

            //|| !string.IsNullOrEmpty(src.FirstName) || !string.IsNullOrEmpty(src.LastName)|| !string.IsNullOrEmpty(src.Email) || !string.IsNullOrEmpty(src.PhoneNumber)));
        }
    }
}
