using Microsoft.AspNetCore.Identity;
using TazaFood.Core.Models.Identity;
using TazaFood.Repository.Identity;

namespace TazaFood_Api.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services) 
        {
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication();
            return services;
        }
    }
}
