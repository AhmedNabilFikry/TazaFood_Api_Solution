
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Identity;
using TazaFood.Repository;
using TazaFood.Repository.Context;
using TazaFood.Repository.Identity;
using TazaFood.Repository.IdentityContext;
using TazaFood.Repository.Repositories;
using TazaFood_Api.Extensions;
using TazaFood_Api.Helpers;

namespace TazaFood_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.
            builder.Services.AddControllers();  // allow Di For Api Services 
            //builder.Services.AddMvc();  allow Di For MVC And Api And Razor Pages 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            //  Add SQL-DataBase Connection("Allow DI")
            builder.Services.AddDbContext<TazaDbContext>(
                Options =>{
                    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //  Add IdentityDbContext Connection("Allow DI")
            builder.Services.AddDbContext<AppIdentityDbContext>(
                Options => {
                    Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));                
                });


            // Add Redis Connection("Allow DI")
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                S => {
                    var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                         return ConnectionMultiplexer.Connect(Connection);
                });

            // Add Application service
            builder.Services.AddApplicationServices();

            builder.Services.UseSwaggerServices();

            // Add Identity Service
            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region Auto Migration

            using ( var scope = app.Services.CreateScope())
            {
                var Services = scope.ServiceProvider;
                // Log The Error In the Kestrel's Console 

                var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
                try
                {
                    // Asking ClR Explicitly TO Create the Object 
                    var dbContext = Services.GetRequiredService<TazaDbContext>();
                    await dbContext.Database.MigrateAsync(); // Apply Migration  "Update-Database"
                    await TazaFoodContextSeeding.SeedAsync(dbContext); // seeding Initil Data To The Database 

                    var IdentityContext = Services.GetRequiredService<AppIdentityDbContext>();
                    await IdentityContext.Database.MigrateAsync(); // Apply Migration  "Update-IdentityDatabase"
                    var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentityDbContextSeed.SeedUserAsync(userManager); // seeding Initil Data To The Database 
                }
                catch (Exception Ex)
                {
                    var Logger = LoggerFactory.CreateLogger<Program>();
                    Logger.LogError(Ex, "An Error Occurred During Apply The Migration");
                }
            }
                #endregion

            #region Configure Kestrel Middlewares
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                app.UseSwaggerMiddlewares();
                }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");    

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers(); 
            #endregion

            app.Run();

        }
    }
}
