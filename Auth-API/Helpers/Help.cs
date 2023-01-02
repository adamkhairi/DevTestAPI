using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Auth_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using ImageUploader;
using Microsoft.Extensions.Options;

namespace Auth_API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class Help
    {
        public static string Check(string oldString, string newString)
        {
            var result = string.IsNullOrEmpty(newString) ? oldString : newString;
            return result;
        }
        
        public static bool UploadImg(byte[] imgByte,string folder)
        {
            var imgStream = new MemoryStream(imgByte != null ? imgByte : new byte[] { });
            var imgName = Guid.NewGuid().ToString();
            var img = $"{imgName}.jpg";
            var productImgFolder = folder;

            return FilesHelper.UploadImage(imgStream, productImgFolder, imgName);
        }
        
        // private readonly UserManager<ApplicationUser> _userManager;
        // private readonly RoleManager<IdentityRole> _roleManager;
        // private readonly IMapper _mapper;
        // private readonly IOptions<Jwt> _jwt;
        //
        // public Help(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<Jwt> jwt)
        // {
        //     _userManager = userManager;
        //     _roleManager = roleManager;
        //     _mapper = mapper;
        //     _jwt = jwt;
        // }
        //
        // public Task<AuthModel> AddUser(RegisterModel model)
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}