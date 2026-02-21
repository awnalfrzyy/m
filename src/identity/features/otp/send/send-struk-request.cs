namespace diggie_server.src.finance.features.receipt.send;

public record SendStrukRequest(
    string Email,
    string ProductName,
    string PlanName,
    decimal Price,
    string PaymentMethod
);