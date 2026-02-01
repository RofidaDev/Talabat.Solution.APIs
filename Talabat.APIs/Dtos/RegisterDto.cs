using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(
        @"(?=^.{6,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}:""'?/>\.<,])(?!.*\s).*$",
        ErrorMessage= "Password must have 1 uppercase, 1 lowercase, 1 number, 1 special character and at least 6 characters"
        )]

        public string Password { get; set; }

    }
}