using diggie_server.src.shop.features.cart.create;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace diggie_server.src.shop.features.cart;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cart")]
// Tambahin : ControllerBase biar bisa pake return Ok()
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly CreateCartHandler _createCartHandler;

    public CartController(ILogger<CartController> logger, CreateCartHandler createCartHandler)
    {
        _logger = logger;
        _createCartHandler = createCartHandler;
    }

    [HttpPost("")]
    public async Task<IActionResult> AddToCart([FromBody] CreateCartRequest request)
    {
        _logger.LogInformation("Menambahkan produk {ProductId} ke keranjang", request.ProductId);

        try
        {
            // Panggil Handler yang udah kita buat tadi
            var response = await _createCartHandler.Handle(request);

            // Balikin 200 OK beserta data lengkap (Name, Image, Price) hasil Join
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal menambahkan item ke keranjang");
            return BadRequest(new { message = ex.Message });
        }
    }
}