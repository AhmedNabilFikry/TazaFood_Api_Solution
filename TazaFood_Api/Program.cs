
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Repository;
using TazaFood.Repository.Context;
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

            #region Configure Service
            // Add services to the container.

            builder.Services.AddControllers();  // allow Di For Api Services 
            //builder.Services.AddMvc();  allow Di For MVC And Api And Razor Pages 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.UseSwaggerServices();

            builder.Services.AddDbContext<TazaDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddApplicationServices();
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
                    await dbContext.Database.MigrateAsync(); // Apply Migration 
                    await TazaFoodContextSeeding.SeedAsync(dbContext); // seeding Initil Data To The Database 

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
