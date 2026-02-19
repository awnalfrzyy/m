namespace diggie_server.src.features.product;

public class CreateProduct
{
    private readonly ProductRepository repository;

    public CreateProduct(ProductRepository repository) => this.repository = repository;

    public async Task<ResponseProduct> Handle(RequestProduct request)
    {
        var product = new EntityProduct
        {
            Id = Guid.NewGuid(),
            Image = request.Image,
            Name = request.Name,
            Brand = request.Brand,
            Price = request.Price,
            Description = request.Description,
            Quantity = request.Quantity,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(product);

        return new ResponseProduct(
     Id: product.Id,
     Image: product.Image,
     Name: product.Name,
     Brand: product.Brand,
     Description: product.Description,
     Price: product.Price,
     Quantity: product.Quantity,
     Status: product.Status,
     CreatedAt: product.CreatedAt
     );
    }
}