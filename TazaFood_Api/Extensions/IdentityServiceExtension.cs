using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TazaFood.Core.Models.Identity;
using TazaFood.Core.Services;
using TazaFood.Repository.Identity;
using TazaFood.Service.Token;

namespace TazaFood_Api.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services ,IConfiguration configuration) 
        {
            services.AddIdentity<AppUser, IdentityRole>( Options => {

                Options.Password.RequiredLength = 8;
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireUppercase = true;
                Options.Password.RequireLowercase = true;
            }    
                ).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddScoped<ITokenServices, TokenService>();
            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme)*/
                Options =>
                {
                    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(Options => {

                    Options.TokenValidationParameters = new TokenValidationParameters() {

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))      
                    };
                });
            return services;
        }
    }
}
