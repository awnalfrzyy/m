public class ErrorRequest
{
    /// <summary>
    /// Exception object yang akan di-log
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Severity level: Critical, Error, Warning, Information
    /// </summary>
    public string? Severity { get; set; }

    /// <summary>
    /// Nama feature/module yang mengalami error (contoh: ProductCreate, OrderProcessing)
    /// </summary>
    public string? FeatureName { get; set; }

    /// <summary>
    /// User ID yang melakukan action, default: Anonymous
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Context data tambahan untuk tracking (contoh: OrderId, ProductId, etc)
    /// </summary>
    public Dictionary<string, object>? ContextData { get; set; }
}
