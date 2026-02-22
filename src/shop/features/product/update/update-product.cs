using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shop.features.product.update;

public class UpdateProduct
{
    private readonly ProductRepository repository;
    public UpdateProduct(ProductRepository repository) => this.repository = repository;

    public async Task<UpdateProductResponse> HandleAsync(UpdateRequestProduct request, Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) throw new Exception("Product tidak ditemukan");

        if (!string.IsNullOrWhiteSpace(request.Image)) product.Image = request.Image!;
        if (!string.IsNullOrWhiteSpace(request.Name)) product.Name = request.Name!;
        if (!string.IsNullOrWhiteSpace(request.Brand)) product.Brand = request.Brand!;
        if (!string.IsNullOrWhiteSpace(request.Description)) product.Description = request.Description!;

        if (request.Price.HasValue)
        {
            if (request.Price.Value < 1000) throw new Exception("Harga minimal Rp 1.000");
            product.Price = request.Price.Value;
        }

        if (request.Quantity.HasValue)
        {
            if (request.Quantity.Value < 0) throw new Exception("Quantity tidak boleh negatif");
            product.Quantity = request.Quantity.Value;
        }
        await repository.UpdateByIdAsync(id, product);


        return new UpdateProductResponse(
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