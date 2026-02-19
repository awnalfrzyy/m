using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.features.product;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product")]
public class ProductController : ControllerBase
{
    private readonly CreateProduct createProduct;

    public ProductController(CreateProduct createProduct) => this.createProduct = createProduct;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] RequestProduct request)
    {
        var response = await createProduct.Handle(request);
        return Ok(response);
    }
}
