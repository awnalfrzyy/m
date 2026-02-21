using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shop.features.product.get;

public class GetProduct
{
    private readonly ProductRepository repository;
    public GetProduct(ProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetProductResponse> ExecuteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        return new GetProductResponse(
            product.Id,
            product.Image,
            product.Name,
            product.Brand,
            product.Description,
            product.Price,
            product.Quantity,
            product.Status,
            product.CreatedAt,
            product.DeleteAt
        );
    }

    public async Task<IEnumerable<GetProductResponse>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(product => new GetProductResponse(
            product.Id,
            product.Image,
            product.Name,
            product.Brand,
            product.Description,
            product.Price,
            product.Quantity,
            product.Status,
            product.CreatedAt,
            product.DeleteAt
        ));
    }
}