using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;
using TazaFood.Core.Services;
using TazaFood.Core.Specification.Order_Specifications;

namespace TazaFood.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;

            //_productRepo = ProductRepo;
            //_deliveryMethodRepo = DeliveryMethodRepo;
            //_orderRepo = OrderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, Address ShippingAddress)
        {
            // 1. Get Basket Form _BasketRepo
            var basket = await _basketRepository.GetUserBasketAsync(BasketId);

            // 2. Get Selected Items At Basket Form _ProductRepo
            var orderItems = new List<OrderItem>();

            // If Basket is not Empty
            if (basket?.items?.Count() > 0)  
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIDAsync(item.ID);
                    var productOrderedItem = new ProductItemOrdered(product.ID , product.Name , product.ImageUrl);
                    var orderItem = new OrderItem(productOrderedItem , product.Price , item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal 
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get DeliveryMethod From _DeliveryMethodRepo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(DeliveryMethod);

            // 5. Create Order
            var Spec = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }
            var order = new Order(BuyerEmail , ShippingAddress , deliveryMethod , orderItems , subTotal, basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().Add(order);

            // 6. Save Changes By IUnit Of Work 
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;
            return order;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int OrderId, string BuyerEmail)
        {
            var Spec = new OrderSpecifications(OrderId, BuyerEmail);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string BuyerEmail)
        {
            var Spec = new OrderSpecifications(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecASync(Spec);
            return Orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllASync();
            return deliveryMethod;
        }

    }
}
