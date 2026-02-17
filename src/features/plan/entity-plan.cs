namespace diggie_server.src.features.plan;

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
    public string Name { get; set; }
    public string Durations { get; set; }
    public decimal Price { get; set; }
    public PlanStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}
