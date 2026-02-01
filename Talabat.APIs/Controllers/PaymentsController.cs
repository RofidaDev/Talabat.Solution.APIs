using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_4fdb7f3674b069d8796c2e19a4143d67800bbd7b56e4c815745fc06f63aefb26";
        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [HttpPost("{basketId}")]  //api/Payments/basketId
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdetePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "An error with your basket"));
            return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
          
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

              
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"],_whSecret);
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Order order;
                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id,true);
                        _logger.LogInformation("Payment is succeeded",paymentIntent.Id);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment is failed", paymentIntent.Id);
                        break;
                }
                  
                    return new EmptyResult();
                }
               
            }
        }
    

