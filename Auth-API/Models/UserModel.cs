using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [Required] [StringLength(100)] public string FirstName { get; set; }

        [Required] [StringLength(100)] public string LastName { get; set; }

        [Required] [StringLength(50)] public string UserName { get; set; }

        [Required,]
        [StringLength(128)]
        [EmailAddress]
        public string Email { get; set; }

        [Required] public string Gender { get; set; }

        [Required] public string Address { get; set; }

        [Required] public string Country { get; set; }

        [Required] public string State { get; set; }

        [Required] public string City { get; set; }

        [Required] public string ZipCode { get; set; }

        public string Message { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 4)]
        public string Password { get; set; }
    }
}