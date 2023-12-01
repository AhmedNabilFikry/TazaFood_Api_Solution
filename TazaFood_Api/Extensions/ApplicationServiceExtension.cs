using TazaFood.Core.IRepository;
using TazaFood.Repository.Repositories;
using TazaFood_Api.Helpers;

namespace TazaFood_Api.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Allow Di For GenericRepository 
            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>(); Per Model
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Allow DI For CategoryRepository
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));

            // Allow DI For Auto Mapper 
            //builder.Services.AddAutoMapper( M => M.AddProfile(new MappingProfiles()));
            // Or By Using a simple Syntax 
            services.AddAutoMapper(typeof(MappingProfiles));


            return services;
        }
    }
}
