using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models.Identity;
using TazaFood.Core.Services;

namespace TazaFood.Service.Token
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            
            if (_configuration == null)
            {
                throw new ArgumentNullException(nameof(_configuration));
            }

            // Add Validation For Required Configuration in AppSettings
            var Key = _configuration["Jwt:Key"];
            var validIssuer = _configuration["Jwt:ValidIssuer"];
            var validAudience = _configuration["Jwt:ValidAudience"];
            var expireDate = _configuration["Jwt:ExpiresDays"];

            if (string.IsNullOrEmpty(validIssuer) || string.IsNullOrEmpty(validAudience) || string.IsNullOrEmpty(expireDate) || string.IsNullOrEmpty(Key))
            {
                throw new InvalidOperationException("Token configuration values are missing or invalid.");
            }

            // AddPrivate Claims 
            var authClaims = new List<Claim>() { 
                
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add Roles
            var userRole = await userManager.GetRolesAsync(user);
            foreach (var role in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
             
            // Add Secret Key     // Defining It In AppSettings
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
          
            // Token Creation
            var Token = new JwtSecurityToken(
                
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:ExpiresDays"])),
                claims:authClaims,
                signingCredentials:new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)
                
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
