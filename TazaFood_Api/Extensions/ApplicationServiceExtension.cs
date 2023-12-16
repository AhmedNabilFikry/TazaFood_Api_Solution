using TazaFood.Core.IRepository;
using TazaFood.Core.Services;
using TazaFood.Repository.Repositories;
using TazaFood.Service;
using TazaFood.Service.CacheService;
using TazaFood.Service.OrderService;
using TazaFood.Service.PaymentService;
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

            // Allow Di For Basket Controller
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // Allow Di for IUnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Allow Di For orderService
            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            // Allow Di For PaymentService
            services.AddScoped<IPaymentService , PaymentService>();

            // Allow Di For CorsPolicy
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", CorsOptions =>
                {
                    CorsOptions.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            //Allow Di For IResponseCachingService
            services.AddSingleton<IResponseCachingService, ResponseCacheService>();

            //Allow Di For IFileService
            services.AddScoped<IFileService ,FileService>();
            return services;
        }
    }
}
