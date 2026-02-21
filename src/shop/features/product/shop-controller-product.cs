using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.shop.features.product.get;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product")]
public class ProductController : ControllerBase
{
    private readonly GetProduct getProduct;
    public ProductController(
        GetProduct getProduct
        )
    {
        this.getProduct = getProduct;
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var response = await getProduct.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await getProduct.ExecuteAsync(id);
        return Ok(response);
    }

}
