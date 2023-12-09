using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail , string BasketId , int DeliveryMethod , Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string BuyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int OrderId, string BuyerEmail);
    }
}
