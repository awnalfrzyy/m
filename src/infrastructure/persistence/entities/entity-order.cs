namespace diggie_server.src.infrastructure.persistence.entities;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}


public class EntityOrder
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; }

    public DateTime? DeleteAt { get; set; }
    public virtual ICollection<EntityOrderItem> OrderItems { get; set; } = new List<EntityOrderItem>();
}