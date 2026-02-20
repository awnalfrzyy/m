using diggie_server.src.features.product.entity;

public record ResponseProduct(
    Guid Id,
    string Image,
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int Quantity,
    ProductStatus Status,
    DateTime CreatedAt,
    DateTime? DeleteAt
);