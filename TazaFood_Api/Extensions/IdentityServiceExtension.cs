using Microsoft.AspNetCore.Identity;
using TazaFood.Core.Models.Identity;
using TazaFood.Repository.Identity;

namespace TazaFood_Api.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services) 
        {
            services.AddIdentity<AppUser, IdentityRole>( Options => {

                Options.Password.RequiredLength = 8;
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireUppercase = true;
                Options.Password.RequireLowercase = true;
            }    
                ).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication();
            return services;
        }
    }
}
