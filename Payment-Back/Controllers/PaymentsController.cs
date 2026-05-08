using Microsoft.AspNetCore.Mvc;
using Payment_Back.Application.DTOs;
using Payment_Back.Application.Interfaces;

namespace Payment_Back.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        // CREATE PAYMENT
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentRequest request)
        {
            var result = await _service.CreatePayment(request);
            return Ok(result);
        }

        // REFUND PAYMENT
        [HttpPost("refund/{externalId}")]
        public async Task<IActionResult> Refund(string externalId)
        {
            await _service.RefundPayment(externalId);
            return Ok("Refund successful");
        }

        // RELEASE ESCROW

        [HttpPost("release/{paymentId}")]
        public async Task<IActionResult> Release(Guid paymentId)
        {
            await _service.ReleaseEscrow(paymentId);
            return Ok("Escrow released");
        }
    }
}