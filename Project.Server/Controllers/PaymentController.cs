using Microsoft.AspNetCore.Mvc;
using Project.BusinessLogic.Service;
using Project.Models.Api;

namespace Project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payment details.");

            var result = await _paymentService.ProcessPaymentAsync(request);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = "Payment successful.",
                    TransactionId = result.TransactionId
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Payment failed.",
                    Error = result.ErrorMessage
                });
            }
        }
    }

}
