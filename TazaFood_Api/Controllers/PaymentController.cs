using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood_Api.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        const string endpointSecret = "whsec_42e72a3134b91a70658a93cc288aa0eb8c4a0bb1b456c8510c45c69ad617f9ff";
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {   
            _paymentService = paymentService;
            _logger = logger;
        }
        [HttpPost("CreatePayment")]
        public async Task<ActionResult<UserBasket>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var Basket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (Basket is null) return BadRequest("No Basket With These ID");
            return Ok(Basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                Order order;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentService.UpdatePaymentIntentStatus(paymentIntent.Id, true);
                        _logger.LogInformation("PaymentIntent is Succeeded", paymentIntent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdatePaymentIntentStatus(paymentIntent.Id, false);
                        _logger.LogInformation("PaymentIntent is Failed", paymentIntent.Id);
                        break;
                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
    
}
