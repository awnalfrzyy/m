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
            product.Image,
            product.Name,
            product.Price
        );
    }

    public async Task<IEnumerable<GetProductResponse>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(product => new GetProductResponse(
            product.Image,
            product.Name,
            product.Price
        ));
    }
}