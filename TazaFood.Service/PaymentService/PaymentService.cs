using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;
using TazaFood.Core.Specification.Order_Specifications;
using Product = TazaFood.Core.Models.Product;

namespace TazaFood.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork   )
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeConnection:Secret key"];
            var Basket = await _basketRepository.GetUserBasketAsync(BasketId);
            if (Basket is null) return null;

            var ShippingPrice = 0m;
            // If there is a DeliveryMethod Is Selected  
            if (Basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = deliveryMethod.Cost;
                Basket.ShippingCost = deliveryMethod.Cost;
            }

            //Get Items Price
            if (Basket?.items?.Count() > 0)
            {
                foreach (var item in Basket.items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIDAsync(item.ID);
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            PaymentIntent paymentIntent;
            var service = new PaymentIntentService();

            // If There's no Payment , Then Create a PaymentIntent 
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                // Create PaymentIntent With Options
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)Basket.items.Sum(items => items.Price * items.Quantity * 100) + (long)ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await service.CreateAsync(options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            // if There's A PaymentIntent 
            else {

                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Basket.items.Sum(items => items.Price * items.Quantity * 100) + (long)ShippingPrice * 100
                };

                await service.UpdateAsync(Basket.PaymentIntentId,options);
            }

            // To track changes that apply to the basket
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentStatus(string PaymentIntentId, bool IsSucceeded)
        {
            var Spec = new OrderWithPaymentIntentIdSpecifications(PaymentIntentId);
            var Order = await _unitOfWork.Repository<Order>().GetByIDWithSpecAsync(Spec);
            if (IsSucceeded)
            {
                Order.Status = OrderStatus.PaymentReceived;
            }
            else 
            {
                Order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(Order);
            await _unitOfWork.Complete();
            return Order;
        }
    }
}
