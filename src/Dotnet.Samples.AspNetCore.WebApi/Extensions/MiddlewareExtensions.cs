using Dotnet.Samples.AspNetCore.WebApi.Middlewares;

namespace Dotnet.Samples.AspNetCore.WebApi.Extensions;

/// <summary>
/// Extension methods for configuring middleware in the application pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware to the application pipeline.
    /// This middleware catches unhandled exceptions and returns RFC 7807 compliant error responses.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <returns>The web application builder for method chaining.</returns>
    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
