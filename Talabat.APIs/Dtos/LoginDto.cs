using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
