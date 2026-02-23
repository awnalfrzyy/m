public enum StatusPayments
{
    Pending,
    Success,
    Failed
}

public class EntityHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string MetodePayments { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public StatusPayments Status { get; set; }
    public DateTime CreatedAt
    { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteAt { get; set; }
}