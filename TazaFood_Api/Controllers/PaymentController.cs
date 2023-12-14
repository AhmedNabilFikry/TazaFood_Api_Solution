using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;

namespace TazaFood_Api.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {   
            _paymentService = paymentService;
        }
        [HttpPost("CreatePayment")]
        public async Task<ActionResult<UserBasket>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var Basket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (Basket is null) return BadRequest("No Basket With These ID");
            return Ok(Basket);
        }
    }
}
