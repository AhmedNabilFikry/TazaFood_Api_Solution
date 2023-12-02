using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;

namespace TazaFood.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        // Asking Clr To create an Obj 
        public BasketRepository(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase(); 
        }
        public async Task<bool> DeleteBasketAsync(string ID)
        {
            return await _database.KeyDeleteAsync(ID);
        }

        public async Task<UserBasket?> GetUserBasketAsync(string ID)
        {
            var Basket = await _database.StringGetAsync(ID);
            return Basket.IsNull ? null :JsonSerializer.Deserialize<UserBasket>(Basket);
        }

        public async Task<UserBasket?> UpdateBasketAsync(UserBasket basket)
        {
            var UpdatedOrCreatedBasket = await _database.StringSetAsync(
                basket.ID, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1)
                );
            if (UpdatedOrCreatedBasket is false) return null;
            return await GetUserBasketAsync(basket.ID);
        }
    }
}
