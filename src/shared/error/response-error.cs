public class ErrorResponse
{
    /// <summary>
    /// Apakah error berhasil di-log
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Status message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Unique trace ID untuk tracking across logs
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Timestamp kapan error di-log (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Feature name yang mengalami error
    /// </summary>
    public string? FeatureName { get; set; }

    /// <summary>
    /// Severity level dari error
    /// </summary>
    public string? Severity { get; set; }

    /// <summary>
    /// Tipe exception yang terjadi
    /// </summary>
    public string? ExceptionType { get; set; }

    /// <summary>
    /// Pesan exception
    /// </summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    /// Stack trace untuk debugging
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// User ID yang mengalami error
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Jumlah context data yang ter-log
    /// </summary>
    public int ContextDataCount { get; set; }
}