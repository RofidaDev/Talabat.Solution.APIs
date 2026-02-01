using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser :IdentityUser  //its id as a guid
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
