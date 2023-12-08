using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood.Repository.Context
{
    public class TazaDbContext :DbContext
    {
        // Asking anyone Who Creates an Obj From TazaDbContext To send Options While Doing A Constructor Chaining  
        public TazaDbContext( DbContextOptions<TazaDbContext> options):base(options)
        {
            
        }
        // I am Gonna Use The Configuration Class
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Appling Configurations From Class That Implements IEntityTypeConfigurations            
            //modelBuilder.ApplyConfiguration(new ProductConfigurations()); //Per Class which Is Not A Solution 
            // Appling Config... from All Classes That Implements IEntityTypeConfig...
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    }
}
