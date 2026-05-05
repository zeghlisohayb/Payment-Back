using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentRequest request)
        {
            var result = await _service.CreatePayment(request);
            return Ok(result);
        }
    }
}
