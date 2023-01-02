using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Auth_API.Models
{
    public class EditUserModel
    {
        public string Id { get; set; }
        [AllowNull] public string FirstName { get; set; }
        [AllowNull] public string LastName { get; set; }
        [AllowNull] public string UserName { get; set; }
        [AllowNull] public string Email { get; set; }
        [AllowNull] public string PhoneNumber { get; set; }
        [AllowNull] public DateTime BirthDate { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string State { get; set; }
        [Required] public string City { get; set; }
        [Required] public string ZipCode { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [JsonIgnore]
        [AllowNull]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [JsonIgnore]
        [AllowNull]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords are not identical")]
        public string ConfirmNewPassword { get; set; }

        public string Message { get; set; }
    }
}