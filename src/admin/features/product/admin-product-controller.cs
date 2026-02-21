using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.admin.features.product.create;
using diggie_server.src.admin.features.product.update;
using diggie_server.src.admin.features.product.delete;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product")]
public class AdminProductController : ControllerBase
{
    private readonly CreateProduct createProduct;
    private readonly UpdateProduct updateProduct;
    private readonly DeleteProduct deleteProduct;
    public AdminProductController(
        CreateProduct createProduct,
        UpdateProduct updateProduct,
        DeleteProduct deleteProduct
        )
    {
        this.createProduct = createProduct;
        this.updateProduct = updateProduct;
        this.deleteProduct = deleteProduct;
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
