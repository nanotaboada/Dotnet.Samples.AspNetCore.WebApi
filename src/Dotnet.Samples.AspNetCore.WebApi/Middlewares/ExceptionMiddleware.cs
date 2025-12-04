using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Middlewares;

/// <summary>
/// Middleware for global exception handling with RFC 7807 Problem Details format.
/// </summary>
public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
{
    private const string ProblemDetailsContentType = "application/problem+json";

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Invokes the middleware to handle exceptions globally.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    /// Handles the exception and returns an RFC 7807 compliant error response.
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = MapExceptionToStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{status}",
            Title = title,
            Status = status,
            Detail = GetExceptionDetail(exception),
            Instance = context.Request.Path
        };

        // Add trace ID for request correlation
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        // Log the exception with structured logging
        logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, StatusCode: {StatusCode}",
            context.TraceIdentifier,
            context.Request.Path,
            status
        );

        // Only modify response if headers haven't been sent yet
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = status;
            context.Response.ContentType = ProblemDetailsContentType;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(problemDetails, JsonOptions)
            );
        }
        else
        {
            logger.LogWarning(
                "Unable to write error response for TraceId: {TraceId}. Response has already started.",
                context.TraceIdentifier
            );
        }
    }

    /// <summary>
    /// Maps exception types to appropriate HTTP status codes and titles.
    /// </summary>
    private static (int StatusCode, string Title) MapExceptionToStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
            ArgumentException
            or ArgumentNullException
                => (StatusCodes.Status400BadRequest, "Bad Request"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid Operation"),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Concurrency Conflict"),
            OperationCanceledException => (StatusCodes.Status408RequestTimeout, "Request Timeout"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }

    /// <summary>
    /// Gets the exception detail based on the environment.
    /// In Development: includes full exception details and stack trace.
    /// In Production: returns a generic message without sensitive information.
    /// </summary>
    private string GetExceptionDetail(Exception exception)
    {
        if (environment.IsDevelopment())
        {
            return $"{exception.Message}\n\nStack Trace:\n{exception.StackTrace}";
        }

        return exception switch
        {
            ValidationException => exception.Message,
            ArgumentException => exception.Message,
            _ => "An unexpected error occurred while processing your request."
        };
    }
}
