using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.IRepository
{
    public interface IBasketRepository
    {
        Task<UserBasket?> GetUserBasketAsync (string ID);
        Task<UserBasket?> UpdateBasketAsync (UserBasket basket);
        Task<bool> DeleteBasketAsync (string ID);
    }
}
