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
    Inactive,
    IsBanned
}



public class EntityUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public GenderUser Gender { get; set; }
    public required string Password { get; set; }
    public StatusUser Status { get; set; } = StatusUser.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeleteAt { get; set; }

    public bool IsPasswordValid(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, this.Password);
    }

    public void UpdatePassword(string newPlainPassword)
    {
        if (string.IsNullOrWhiteSpace(newPlainPassword))
            throw new ArgumentException("Password tidak boleh kosong.");

        this.Password = BCrypt.Net.BCrypt.HashPassword(newPlainPassword);
    }

    public void EnsureCanAccessSystem()
    {
        if (this.DeleteAt != null)
        {
            throw new KeyNotFoundException("Akun tidak ditemukan atau telah dihapus.");
        }

        if (this.Status == StatusUser.IsBanned)
        {
            throw new UnauthorizedAccessException("Akun kamu ditangguhkan (Banned).");
        }

        if (this.Status == StatusUser.Inactive)
        {
            throw new UnauthorizedAccessException("Akun non-aktif. Silakan hubungi admin.");
        }
    }
}