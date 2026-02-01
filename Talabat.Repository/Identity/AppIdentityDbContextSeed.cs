using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public async static Task SeedAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)  //if table in Identity DB is empty
                                                  //Users looks like DbSet<AppUser>
            {
                var user = new AppUser()
                {
                    DisplayName = "Rofida",
                    Email = "rofyh8890gmail.com",
                    UserName = "RofidaDiv",
                    PhoneNumber = "01028485615"
               
                };
                await _userManager.CreateAsync(user,"562003R@fy");
                
            }
        }
    }
}
