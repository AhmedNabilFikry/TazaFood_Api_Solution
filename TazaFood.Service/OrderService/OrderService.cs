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

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;

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
            var order = new Order(BuyerEmail , ShippingAddress , deliveryMethod , orderItems , subTotal);
            await _unitOfWork.Repository<Order>().Add(order);

            // 6. Save Changes By IUnit Of Work 
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;
            return order;
        }

        public Task<Order> GetOrderByIdForUserAsync(int OrderId, string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string BuyerEmail)
        {
            var Spec = new OrderSpecifications(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecASync(Spec);
            return Orders;
        }
    }
}
