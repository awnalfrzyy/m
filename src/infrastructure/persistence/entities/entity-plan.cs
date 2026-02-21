namespace diggie_server.src.infrastructure.persistence.entities;

public enum PlanStatus
{
    Active = 0,
    Inactive = 1,
    Deleted = 2
}

public class EntityPlan
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public required string Name { get; set; }
    public required string Durations { get; set; }
    public decimal Price { get; set; }
    public PlanStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}
