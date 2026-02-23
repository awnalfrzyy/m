using diggie_server.src.infrastructure.persistence.entities;
public class EntityOrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeleteAt { get; set; }
    public virtual EntityOrder Order { get; set; } = null!;
    public virtual EntityProduct Product { get; set; } = null!;
}