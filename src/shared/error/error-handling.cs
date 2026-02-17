namespace diggie_server.src.shared.error
{
    /// <summary>
    /// Error Handler Logger - Handling error dengan clarity dan non-blocking
    /// 
    /// USAGE EXAMPLES:
    /// ===============
    /// 1. Basic Error Logging:
    ///    var errorHandler = new ErrorHandlerLogger(_logger);
    ///    await errorHandler.LogErrorAsync(new ErrorRequest 
    ///    { 
    ///        Exception = ex,
    ///        Severity = "Error",
    ///        FeatureName = "ProductCreate",
    ///        UserId = "user123"
    ///    });
    /// 
    /// 2. Logging dengan Context Data:
    ///    await errorHandler.LogErrorAsync(new ErrorRequest 
    ///    { 
    ///        Exception = ex,
    ///        Severity = "Critical",
    ///        FeatureName = "OrderProcessing",
    ///        UserId = "user456",
    ///        ContextData = new Dictionary&lt;string, object&gt;
    ///        {
    ///            { "OrderId", "ORD-123456" },
    ///            { "Amount", 500000 },
    ///            { "PaymentMethod", "Credit Card" }
    ///        }
    ///    });
    /// 
    /// 3. Mendapatkan Error Response:
    ///    var response = await errorHandler.LogErrorAsync(errorRequest);
    ///    if (response.IsSuccess)
    ///    {
    ///        Console.WriteLine($"Error logged dengan TraceId: {response.TraceId}");
    ///    }
    /// 
    /// 4. Logging Validation Errors:
    ///    await errorHandler.LogErrorAsync(new ErrorRequest
    ///    {
    ///        Exception = new InvalidOperationException("Invalid email format"),
    ///        Severity = "Warning",
    ///        FeatureName = "UserRegistration",
    ///        ContextData = new Dictionary&lt;string, object&gt;
    ///        {
    ///            { "FieldName", "Email" },
    ///            { "Input", "invalid-email" }
    ///        }
    ///    });
    /// </summary>
    public class ErrorHandlerLogger
    {
        private readonly ILogger<ErrorHandlerLogger> _logger;
        private readonly string _applicationName;

        public ErrorHandlerLogger(ILogger<ErrorHandlerLogger> logger, string applicationName = "DiggieServer")
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationName = applicationName;
        }

        /// <summary>
        /// Log error secara async dan non-blocking
        /// </summary>
        public async Task<ErrorResponse> LogErrorAsync(ErrorRequest request)
        {
            try
            {
                // Validate request
                if (request == null)
                {
                    return CreateFailureResponse("Error request tidak boleh null");
                }

                // Generate unique trace ID untuk tracking
                var traceId = GenerateTraceId();
                var logTimestamp = DateTime.UtcNow;

                // Build structured log message (non-blocking)
                var logData = BuildLogData(request, traceId, logTimestamp);

                // Log secara async non-blocking
                await Task.Run(() =>
                {
                    switch (request.Severity?.ToLower())
                    {
                        case "critical":
                            _logger.LogCritical(request.Exception, BuildLogMessage(logData));
                            break;
                        case "error":
                            _logger.LogError(request.Exception, BuildLogMessage(logData));
                            break;
                        case "warning":
                            _logger.LogWarning(request.Exception, BuildLogMessage(logData));
                            break;
                        case "information":
                        case "info":
                            _logger.LogInformation(BuildLogMessage(logData));
                            break;
                        default:
                            _logger.LogError(request.Exception, BuildLogMessage(logData));
                            break;
                    }
                });

                // Return success response dengan full data
                return new ErrorResponse
                {
                    IsSuccess = true,
                    Message = "Error berhasil dicatat",
                    TraceId = traceId,
                    Timestamp = logTimestamp,
                    FeatureName = request.FeatureName,
                    Severity = request.Severity,
                    ExceptionType = request.Exception?.GetType().Name,
                    ExceptionMessage = request.Exception?.Message,
                    StackTrace = request.Exception?.StackTrace,
                    UserId = request.UserId,
                    ContextDataCount = request.ContextData?.Count ?? 0
                };
            }
            catch (Exception ex)
            {
                // Fail-safe untuk mencegah error handler sendiri crash
                _logger.LogError(ex, "Error terjadi di dalam ErrorHandlerLogger");
                return CreateFailureResponse($"Gagal mencatat error: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate unique trace ID untuk tracking across logs
        /// </summary>
        private string GenerateTraceId()
        {
            return $"{_applicationName}-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        /// <summary>
        /// Build structured log data dari request
        /// </summary>
        private Dictionary<string, object> BuildLogData(ErrorRequest request, string traceId, DateTime timestamp)
        {
            var logData = new Dictionary<string, object>
            {
                { "TraceId", traceId },
                { "Timestamp", timestamp },
                { "ApplicationName", _applicationName },
                { "Severity", request.Severity ?? "Error" },
                { "FeatureName", request.FeatureName ?? "Unknown" },
                { "UserId", request.UserId ?? "Anonymous" },
                { "ExceptionType", request.Exception?.GetType().Name ?? "Unknown" },
                { "ExceptionMessage", request.Exception?.Message ?? "No message" },
                { "StackTrace", request.Exception?.StackTrace ?? "" }
            };

            // Add custom context data
            if (request.ContextData != null && request.ContextData.Count > 0)
            {
                var contextIndex = 1;
                foreach (var context in request.ContextData)
                {
                    logData[$"ContextData_{contextIndex}_{context.Key}"] = context.Value ?? "null";
                    contextIndex++;
                }
            }

            return logData;
        }

        /// <summary>
        /// Build readable log message dari log data
        /// </summary>
        private string BuildLogMessage(Dictionary<string, object> logData)
        {
            var message = $"\n=== ERROR LOG ===\n";
            foreach (var item in logData)
            {
                message += $"{item.Key}: {item.Value}\n";
            }
            message += $"=================\n";
            return message;
        }

        /// <summary>
        /// Create failure response untuk error case
        /// </summary>
        private ErrorResponse CreateFailureResponse(string message)
        {
            return new ErrorResponse
            {
                IsSuccess = false,
                Message = message,
                TraceId = GenerateTraceId(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
