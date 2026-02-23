using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shop.features.product.get.detail;

public class GetProductDetail
{
    private readonly ProductRepository repository;
    public GetProductDetail(ProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetProductDetailResponse> ExecuteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        return new GetProductDetailResponse(
            product.Image,
            product.Name,
            product.Brand,
            product.Description,
            product.Price,
            product.Quantity
        );
    }

    public async Task<IEnumerable<GetProductDetailResponse>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(product => new GetProductDetailResponse(
            product.Image,
            product.Name,
            product.Brand,
            product.Description,
            product.Price,
            product.Quantity
        ));
    }
}