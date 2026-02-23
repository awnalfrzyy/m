using diggie_server.src.infrastructure.persistence.entities;

public record GetProductDetailResponse(
    string Image,
    string Name,
    string? Brand,
    string? Description,
    decimal Price,
    int? Quantity
);