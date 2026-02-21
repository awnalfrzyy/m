using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.admin.features.product.delete;

public class DeleteProduct
{
    private readonly ProductRepository repository;

    public DeleteProduct(ProductRepository repository) => this.repository = repository;

    public async Task<DeleteProductResponse> HandleAsync(Guid id)
    {
        var product = await repository.DeleteByIdAsync(id);
        return new DeleteProductResponse(
            Id: product.Id,
            Image: product.Image,
            Name: product.Name,
            Brand: product.Brand,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            Status: product.Status,
            CreatedAt: product.CreatedAt,
            DeleteAt: product.DeleteAt
        );
    }
}