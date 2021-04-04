using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Auth_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required ,MaxLength(50)]
        public string FirstName { get; set; }


        [Required, MaxLength(50)]
        public string LastName { get; set; }
    }
}
