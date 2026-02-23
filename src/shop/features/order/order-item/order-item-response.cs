public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal PriceAtPurchase
);