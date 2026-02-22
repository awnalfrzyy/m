using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.shop.features.product.get;
using diggie_server.src.shop.features.product.create;
using diggie_server.src.shop.features.product.update;
using diggie_server.src.shop.features.product.delete;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product")]
public class ProductController : ControllerBase
{
    private readonly GetProduct getProduct;
    private readonly CreateProduct createProduct;
    private readonly UpdateProduct updateProduct;
    private readonly DeleteProduct deleteProduct;

    public ProductController(
        GetProduct getProduct,
        CreateProduct createProduct,
        UpdateProduct updateProduct,
        DeleteProduct deleteProduct
        )
    {
        this.getProduct = getProduct;
        this.createProduct = createProduct;
        this.updateProduct = updateProduct;
        this.deleteProduct = deleteProduct;
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

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var response = await createProduct.Handle(request);
        return Ok(response);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequestProduct request)
    {
        var response = await updateProduct.HandleAsync(request, id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await deleteProduct.HandleAsync(id);
        return Ok("Product deleted successfully");
    }

}
