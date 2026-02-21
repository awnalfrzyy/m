public enum OtpStatus
{
    Pending = 0,
    Verified = 1,
    Expired = 2,
    Invalidated = 3
}

public class EntityOtp
{
    public required string Email { get; set; }
    public required string Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    public OtpStatus Status { get; set; } = OtpStatus.Pending;
}