using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TazaFood.Core.Models;
using TazaFood.Repository.Context;

namespace TazaFood.Repository
{
    public static class TazaFoodContextSeeding 
    {
        public static async Task SeedAsync(TazaDbContext context)
        {
            // Check if there is Data in Category Table 
            if (!context.Set<Category>().Any())
            {
                // Read the File As Text
                var CategoryData = File.ReadAllText("../TazaFood.Repository/Context/Data Seeding/Categories.json");
                var Categories = JsonSerializer.Deserialize<List<Category>>(CategoryData);
                if (Categories is not null && Categories.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        await context.Set<Category>().AddAsync(category);
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Set<Product>().Any())
            {
                var ProductData = File.ReadAllText("../TazaFood.Repository/Context/Data Seeding/Products.Json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products is not null && Products.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                       await context.Set<Product>().AddAsync(product);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
