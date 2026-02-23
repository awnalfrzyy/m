namespace diggie_server.src.infrastructure.persistence.entities;

public class EntityCart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public virtual EntityProduct Product { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteAt { get; set; }
}