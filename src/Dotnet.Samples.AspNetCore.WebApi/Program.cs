using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

/* -----------------------------------------------------------------------------
 * Web Application
 * Registers all services into the DI container before the application is built.
 * Throughout this section, builder.Services refers to the ASP.NET Core
 * dependency injection (DI) container — not to be confused with the Services
 * subsection below, which registers our own application-level business logic.
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
 * -------------------------------------------------------------------------- */

var builder = WebApplication.CreateBuilder(args);

/* Configurations ----------------------------------------------------------- */

builder
    .Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

/* Logging ------------------------------------------------------------------ */

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

/* Infrastructure ----------------------------------------------------------- */

builder.Services.AddHealthChecks();
builder.Services.AddCorsDefaultPolicy(builder.Environment);
builder.Services.AddFixedWindowRateLimiter(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerConfiguration(builder.Configuration);
}

/* Controllers -------------------------------------------------------------- */

builder.Services.AddControllers();
builder.Services.AddValidators();

/* Services (Business Logic) ------------------------------------------------ */

builder.Services.RegisterPlayerService();
builder.Services.AddMemoryCache();
builder.Services.AddMappings();

/* Repositories ------------------------------------------------------------- */

builder.Services.RegisterPlayerRepository();

/* Data --------------------------------------------------------------------- */

builder.Services.AddDbContextPool(builder.Environment);

var app = builder.Build();

/* -----------------------------------------------------------------------------
 * Database Migration
 * Applies pending EF Core migrations at startup, before the app accepts requests.
 * https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime
 * -------------------------------------------------------------------------- */

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlayerDbContext>();
    await db.Database.MigrateAsync();
}

/* -----------------------------------------------------------------------------
 * Middlewares
 * Defines the order in which middleware components process each HTTP request.
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware#middleware-order
 * -------------------------------------------------------------------------- */

// Replaces the default ASP.NET Core request logging with Serilog's structured
// logging, emitting one log entry per request with timing, status code, and
// other contextual properties.
app.UseSerilogRequestLogging();

// Custom middleware that catches unhandled exceptions and returns a consistent
// RFC 7807 Problem Details response instead of exposing a raw stack trace.
app.UseExceptionHandling();

// Redirects all plain HTTP requests to HTTPS, enforcing transport security.
app.UseHttpsRedirection();

// DisableRateLimiting() exempts the health check endpoint from the global rate
// limiter so that monitoring and orchestration systems can always assess
// liveness and readiness without being throttled.
app.MapHealthChecks("/health").DisableRateLimiting();

// Enforces the fixed-window rate limiting policy defined during service
// registration, returning 429 Too Many Requests when the limit is exceeded.
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    // Only active in Development, where AddCorsDefaultPolicy registers a
    // permissive wildcard policy for Swagger UI and local frontends. No policy
    // exists in Production — the API is assumed to be consumed server-to-server
    // or behind a reverse proxy on the same origin, where CORS is not needed.
    // Must precede MapControllers so CORS headers are applied before any
    // endpoint executes, consistent with the standard middleware pipeline order.
    app.UseCors();

    // Generates the OpenAPI JSON document consumed by Swagger UI.
    app.UseSwagger();

    // Serves the interactive Swagger UI at /swagger, allowing manual
    // exploration and testing of the API endpoints during development.
    app.UseSwaggerUI();
}

// Routes incoming HTTP requests to the matching controller actions. Must come
// after all middleware that needs to run before endpoint execution (CORS, rate
// limiting, etc.).
app.MapControllers();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
