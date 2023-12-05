using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using TazaFood.Core.Models.Identity;
using TazaFood.Core.Services;
using TazaFood.Repository.Identity;
using TazaFood.Service.Token;

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

            services.AddScoped<ITokenServices, TokenService>();
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme
                ).AddJwtBearer();
            return services;
        }
    }
}
