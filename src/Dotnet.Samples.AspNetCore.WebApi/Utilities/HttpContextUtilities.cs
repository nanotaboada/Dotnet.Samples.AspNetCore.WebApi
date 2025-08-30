using System.Net;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities;

/// <summary>
/// Utility class for HTTP context operations.
/// </summary>
public static class HttpContextUtilities
{
    /// <summary>
    /// This method checks for the "X-Forwarded-For" and "X-Real-IP" headers,
    /// which are commonly used by proxies to forward the original client IP address.
    /// If these headers are not present or the IP address cannot be parsed,
    /// it falls back to the remote IP address from the connection.
    /// If no valid IP address can be determined, it returns "unknown".
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The client IP address or "unknown" if not available.</returns>
    public static string ExtractIpAddress(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var headers = httpContext.Request.Headers;
        IPAddress? ipAddress;

        if (headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
        {
            var clientIp = xForwardedFor
                .ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(clientIp) && IPAddress.TryParse(clientIp, out ipAddress))
                return ipAddress.ToString();
        }

        if (
            headers.TryGetValue("X-Real-IP", out var xRealIp)
            && IPAddress.TryParse(xRealIp.ToString(), out ipAddress)
        )
        {
            return ipAddress.ToString();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? $"unknown-{Guid.NewGuid()}";
    }
}
