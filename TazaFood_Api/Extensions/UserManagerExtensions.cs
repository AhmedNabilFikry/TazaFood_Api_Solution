using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TazaFood.Core.Models.Identity;

namespace TazaFood_Api.Extensions
{
    public  static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> _userManager, ClaimsPrincipal User)
        {

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == Email);
            return user;
        }
    }
}
