using diggie_server.src.features.product;

public record ResponseProduct(
    Guid Id,
    string Image,
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int Quantity,
    ProductStatus Status,
    DateTime CreatedAt
);