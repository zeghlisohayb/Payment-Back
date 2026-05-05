using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Payment_Back.Application.Interfaces;

namespace Payment_Back.Controllers
{
    [ApiController]
    [Route("api/webhook/paypal")]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public WebhookController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonElement payload)
        {
            var eventType = payload.GetProperty("event_type").GetString();

            if (eventType == "PAYMENT.CAPTURE.COMPLETED")
            {
                var externalId = payload
                    .GetProperty("resource")
                    .GetProperty("id")
                    .GetString();

                await _paymentService.ConfirmPayment(externalId);
            }

            if (eventType == "PAYMENT.CAPTURE.DENIED")
            {
                var externalId = payload
                    .GetProperty("resource")
                    .GetProperty("id")
                    .GetString();

                await _paymentService.FailPayment(externalId);
            }

            return Ok();
        }
    }
}