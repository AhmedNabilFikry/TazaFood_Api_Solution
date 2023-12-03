using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models.Identity;

namespace TazaFood.Repository.IdentityContext
{
    public class AppIdentityDbContextSeed
    {
        // User Seeding 
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            // Check First If There's No User in the Database
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Nabil",
                    Email = "Admin@gmail.com",
                    UserName = "Ahmed.Nabil",
                    PhoneNumber = "01024280793"
                };

                var result = await userManager.CreateAsync(user, "Admin-123");

                if (result.Succeeded)
                {
                    // Log user information
                    Console.WriteLine($"User seeded successfully:");
                    Console.WriteLine($"User Id: {user.Id}");
                    Console.WriteLine($"User Display Name: {user.DisplayName}");
                    Console.WriteLine($"User Email: {user.Email}");
                    Console.WriteLine($"User UserName: {user.UserName}");
                    Console.WriteLine($"User Phone Number: {user.PhoneNumber}");
                }
                else
                {
                    // Log any errors that occurred during user creation
                    Console.WriteLine("User seeding failed. Errors:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}

