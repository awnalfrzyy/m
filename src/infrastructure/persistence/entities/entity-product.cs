namespace diggie_server.src.infrastructure.persistence.entities;

public enum ProductStatus
{
    Active = 0,
    Inactive = 1,
    Deleted = 2
}

public class EntityProduct
{
    public Guid Id { get; set; }
    public required string Image { get; set; }
    public required string Name { get; set; }
    public required string Brand { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}
