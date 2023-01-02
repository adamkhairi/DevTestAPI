using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Auth_API.Models
{
    public class ApplicationUser : IdentityUser
    {
         
        [Required] [MaxLength(50)] public string FirstName { get; set; }
        [Required] [MaxLength(50)] public string LastName { get; set; }
        [AllowNull] public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

    }
}
