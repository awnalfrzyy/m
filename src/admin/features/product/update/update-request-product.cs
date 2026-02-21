public record UpdateRequestProduct(
    string? Image,
    string? Name,
    string? Brand,
    string? Description,
    decimal? Price,
    int? Quantity
);