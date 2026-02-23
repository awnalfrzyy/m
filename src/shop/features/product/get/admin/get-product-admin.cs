using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shop.features.product.get.admin;

public class GetProductAdmin
{
    private readonly ProductRepository repository;
    public GetProductAdmin(ProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetProductAdminResponse> ExecuteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        return new GetProductAdminResponse(
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

    public async Task<IEnumerable<GetProductAdminResponse>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(product => new GetProductAdminResponse(
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