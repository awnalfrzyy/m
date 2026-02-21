namespace diggie_server.src.infrastructure.persistence.entities;

public enum GenderUser
{
    Male,
    Female,
    Other
}

public enum StatusUser
{
    Active,
    Inactive
}

public class EntityUser
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public GenderUser Gender { get; set; }
    public required string Password { get; set; }
    public StatusUser Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}