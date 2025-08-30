namespace Dotnet.Samples.AspNetCore.WebApi.Configurations;

/// <summary>
/// Configuration options for the Fixed Window Rate Limiter.
/// </summary>
public class RateLimiterConfiguration
{
    /// <summary>
    /// Gets or sets the maximum number of permits that can be leased per window.
    /// Default value is 60 requests.
    /// </summary>
    public int PermitLimit { get; set; } = 60;

    /// <summary>
    /// Gets or sets the time window in seconds for rate limiting.
    /// Default value is 60 seconds (1 minute).
    /// </summary>
    public int WindowSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the maximum number of requests that can be queued when the permit limit is exceeded.
    /// A value of 0 means no queuing (default).
    /// </summary>
    public int QueueLimit { get; set; } = 0;
}
