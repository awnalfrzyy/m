public record ResponsePlan(
    Guid Id,
    Guid ProductId,
    string Name,
    string Durations,
    decimal Price,
    PlanStatus Status,
    DateTime CreatedAt,
    DateTime? DeleteAt
);