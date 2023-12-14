using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood.Core.IRepository
{
    public interface IPaymentService
    {
        Task<UserBasket> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentStatus(string PaymentIntentId, bool IsSucceeded);
    }
}
