using Microsoft.AspNetCore.Identity;

namespace WukkaManCleaning_API.Utils
{
    public static class SeedData
    {
        public static async Task Intialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
        }
    }
}
