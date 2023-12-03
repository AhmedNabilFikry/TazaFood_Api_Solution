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
        public static async Task SeedUserAsync(UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
        {
            try
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
                        var logger = loggerFactory.CreateLogger("UserSeeder");
                        logger.LogInformation("User seeded successfully:");
                        logger.LogInformation($"User Id: {user.Id}");
                        logger.LogInformation($"User Display Name: {user.DisplayName}");
                        logger.LogInformation($"User Email: {user.Email}");
                        logger.LogInformation($"User UserName: {user.UserName}");
                        logger.LogInformation($"User Phone Number: {user.PhoneNumber}");
                    }
                    else
                    {
                        // Log errors
                        var logger = loggerFactory.CreateLogger("UserSeeder");
                        logger.LogError("User seeding failed. Errors:");
                        foreach (var error in result.Errors)
                        {
                            logger.LogError(error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                var logger = loggerFactory.CreateLogger("UserSeeder");
                logger.LogError($"An unexpected error occurred during user seeding: {ex.Message}");
            }
        }

    }
}
}
