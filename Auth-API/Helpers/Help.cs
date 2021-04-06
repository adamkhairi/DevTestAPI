using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Auth_API.Helpers
{
    public class Help : IHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IOptions<Jwt> _jwt;

        public Help(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt;
        }

        public Task<AuthModel> AddUser(RegisterModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}