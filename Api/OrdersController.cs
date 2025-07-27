using IdempotentApi.Abstractions.Attributes;
using IdempotentApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace IdempotentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [IdempotencyFilter(ttlMinutes: 60)]
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || request.Quantity <= 0)
            {
                return BadRequest("Invalid order data.");
            }

            var orderId = Guid.NewGuid();
            var order = new
            {
                Id = orderId,
                request.Name,
                request.Quantity,
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderId }, order);
        }


        [HttpGet]
        public IActionResult GetOrder([FromQuery] Guid id)
        {
            var Id = id == Guid.Empty ? Guid.NewGuid() : id;
            return Ok(new
            {
                Id,
                Name = "Sample Order",
                Quantity = 10,
            });
        }
    }
}
