using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shop.features.cart.create;

public class CreateCartHandler
{
    private readonly RepositoryCart repositoryCart;
    private readonly ProductRepository productRepository;
    private readonly ILogger<CreateCartHandler> logger;
    public CreateCartHandler(RepositoryCart repositoryCart, ProductRepository productRepository, ILogger<CreateCartHandler> logger)
    {
        this.repositoryCart = repositoryCart;
        this.productRepository = productRepository;
        this.logger = logger;
    }
    public async Task<CreateCartResponse> Handle(CreateCartRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null) throw new Exception("item not available");

        var existingCart = await repositoryCart.GetCartItem(request.ProductId);

        if (existingCart != null)
        {
            existingCart.Quantity += request.Quantity;
            await repositoryCart.UpdateCartItem(existingCart);
        }
        else
        {
            var newCart = new EntityCart
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            await repositoryCart.AddCartItem(newCart);
        }

        var response = await repositoryCart.GetCartResponse(request.ProductId);

        return response ?? throw new Exception("failed to retrieve data from cart");
    }
}