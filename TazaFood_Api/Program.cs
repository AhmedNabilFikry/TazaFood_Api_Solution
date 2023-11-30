
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Repository;
using TazaFood.Repository.Context;
using TazaFood.Repository.Repositories;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TazaDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Allow Di For GenericRepository 
            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>(); Per Model
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
