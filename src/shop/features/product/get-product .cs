using diggie_server.src.infrastructure.persistence;

namespace diggie_server.src.shop.features.product;

public class GetProduct
{
    private readonly ProductRepository repository;
    public GetProduct(ProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ResponseProduct> ExecuteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        return new ResponseProduct(
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

    public async Task<IEnumerable<ResponseProduct>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(product => new ResponseProduct(
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