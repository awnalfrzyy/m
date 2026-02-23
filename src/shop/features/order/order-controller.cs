using diggie_server.src.shop.features.order.create;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace diggie_server.src.shop.features.order;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/order")]
public class OrderController : ControllerBase
{
    private readonly CreateOrderHandler _createOrderHandler;
    private readonly ILogger<OrderController> _logger;

    public OrderController(
        CreateOrderHandler createOrderHandler,
        ILogger<OrderController> logger)
    {
        _createOrderHandler = createOrderHandler;
        _logger = logger;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CreateOrderRequest request)
    {
        _logger.LogInformation("HTTP POST: Menerima permintaan Checkout.");

        try
        {
            var response = await _createOrderHandler.handle(request);

            _logger.LogInformation("HTTP POST: Checkout berhasil untuk Order {OrderId}", response.Id);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP POST ERROR: Gagal memproses checkout.");

            return BadRequest(new
            {
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}