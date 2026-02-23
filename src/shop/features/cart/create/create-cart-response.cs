public record CreateCartResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductImage,
    decimal ProductPrice,
    int Quantity,
    decimal SubTotal
);