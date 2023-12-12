using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TazaFood.Core.Models.Order_Aggregate;
using TazaFood.Core.Services;
using TazaFood_Api.Dtos;

namespace TazaFood_Api.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderTOReturnDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BaskektId, orderDto.DeliveryMethod, Address);
            if (Order is null) return BadRequest();
            return Ok(_mapper.Map<Order ,OrderTOReturnDto>(Order));
        }

        [HttpGet("GetOrdersForUser")]
        public async Task<ActionResult<IReadOnlyList<OrderTOReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForUsersAsync(BuyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderTOReturnDto>>(Orders));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderTOReturnDto>> GetOrdersByIdForUser(int Id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForUserAsync(Id , BuyerEmail);
            if (Order is null) return NotFound();
            return Ok(_mapper.Map<Order,OrderTOReturnDto>(Order));
        }

        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var DeliveryMethod = await _orderService.GetDeliveryMethodAsync();
            return Ok(DeliveryMethod);
        }
    }
}
