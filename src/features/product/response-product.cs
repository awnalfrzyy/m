public record ResponseProduct(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    bool Status,
    DateTime CreatedAt
);