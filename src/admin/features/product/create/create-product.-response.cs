using diggie_server.src.infrastructure.persistence.entities;

public record CreateProductResponse(
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