using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Services;
using Project.Models.EntityModels;

namespace Project.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentLogsController : ControllerBase
    {
        private readonly IPaymentLogService _paymentLogService;

        public PaymentLogsController(IPaymentLogService paymentLogService)
        {
            _paymentLogService = paymentLogService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPaymentLog([FromBody] PaymentLog paymentLog)
        {
            var newLog = await _paymentLogService.AddAsync(paymentLog);
            return CreatedAtAction(nameof(GetPaymentLogsByOrderId), new { orderId = newLog.OrderId }, newLog);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentLogsByOrderId(int orderId)
        {
            var logs = await _paymentLogService.GetByIdAsync(orderId);
            return Ok(logs);
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentLogStatus([FromBody] PaymentLog request)
        {
            var updated = await _paymentLogService.UpdateAsync(request);
            return updated != null ? Ok() : NotFound("Payment log not found.");
        }
    }

}
