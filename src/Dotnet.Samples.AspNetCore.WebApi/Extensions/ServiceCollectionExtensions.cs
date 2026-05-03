using System.Threading.RateLimiting;
using Dotnet.Samples.AspNetCore.WebApi.Configurations;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Mappings;
using Dotnet.Samples.AspNetCore.WebApi.Migrations;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi;
using Serilog;

namespace Dotnet.Samples.AspNetCore.WebApi.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to encapsulate service configuration.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DbContextPool for PlayerDbContext, selecting the database provider based on the
    /// <c>DATABASE_PROVIDER</c> environment variable (<c>sqlite</c> by default, <c>postgres</c>
    /// to opt in to PostgreSQL).
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="environment">The web host environment.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddDbContextPool(
        this IServiceCollection services,
        IWebHostEnvironment environment
    )
    {
        services.AddDbContextPool<PlayerDbContext>(options =>
        {
            var provider = (
                Environment.GetEnvironmentVariable("DATABASE_PROVIDER") ?? ""
            )
                .Trim()
                .ToLowerInvariant();

            switch (provider)
            {
                case "postgres":
                    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
                    if (string.IsNullOrWhiteSpace(connectionString))
                        throw new InvalidOperationException(
                            "DATABASE_URL is required when DATABASE_PROVIDER=postgres."
                        );
                    options.UseNpgsql(connectionString);
                    break;

                case "sqlite":
                case "":
                    var storagePath = Environment.GetEnvironmentVariable("STORAGE_PATH");
                    var dataSource = !string.IsNullOrWhiteSpace(storagePath)
                        ? storagePath
                        : Path.Combine(AppContext.BaseDirectory, "storage", "players-sqlite3.db");

                    var storageDir = Path.GetDirectoryName(dataSource);
                    if (!string.IsNullOrWhiteSpace(storageDir))
                    {
                        Directory.CreateDirectory(storageDir);
                    }
                    options.UseSqlite($"Data Source={dataSource}");
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported DATABASE_PROVIDER value: '{provider}'. "
                            + "Valid values are 'sqlite' (default) and 'postgres'."
                    );
            }

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.LogTo(Log.Logger.Information, LogLevel.Information);
            }

            options.ReplaceService<IMigrationsAssembly, ProviderSpecificMigrationsAssembly>();
        });

        return services;
    }

    /// <summary>
    /// Adds a default CORS policy that allows any origin, method, and header,
    /// restricted to the Development environment.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/security/cors"/>
    /// </summary>
    /// <remarks>
    /// The permissive wildcard policy (AllowAnyOrigin, AllowAnyMethod, AllowAnyHeader)
    /// is intentional for local development, where Swagger UI and local frontends
    /// need unrestricted cross-origin access. No CORS policy is registered in
    /// Production or other environments, where the API is assumed to be consumed
    /// server-to-server or to sit behind a reverse proxy on the same origin, making
    /// CORS irrelevant. If a production frontend on a different domain is ever added,
    /// replace this with a restrictive named policy that enumerates specific allowed
    /// origins instead of using a wildcard.
    /// </remarks>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="environment">The web host environment.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddCorsDefaultPolicy(
        this IServiceCollection services,
        IWebHostEnvironment environment
    )
    {
        if (environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(corsBuilder =>
                {
                    corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        // No CORS configured in Production or other environments

        return services;
    }

    /// <summary>
    /// Adds FluentValidation validators for Player models.
    /// <br />
    /// <see href="https://docs.fluentvalidation.net/en/latest/aspnet.html"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<PlayerRequestModelValidator>();
        return services;
    }

    /// <summary>
    /// Sets up Swagger documentation generation and UI for the API.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle" />
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSwaggerGen(options =>
        {
            var openApiInfo = configuration.GetSection("OpenApiInfo").Get<OpenApiInfo>();

            options.SwaggerDoc("v1", openApiInfo);
            options.IncludeXmlComments(SwaggerUtilities.ConfigureXmlCommentsFilePath());
            options.AddSecurityDefinition("Bearer", SwaggerUtilities.ConfigureSecurityDefinition());
            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    /// <summary>
    /// Registers the PlayerService with the DI container.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection RegisterPlayerService(this IServiceCollection services)
    {
        services.AddScoped<IPlayerService, PlayerService>();
        return services;
    }

    /// <summary>
    /// Adds AutoMapper configuration for Player mappings.
    /// <br />
    /// <see href="https://docs.automapper.io/en/latest/Dependency-injection.html#asp-net-core"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile<PlayerMappingProfile>());
        return services;
    }

    /// <summary>
    /// Registers the PlayerRepository service with the DI container.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection RegisterPlayerRepository(this IServiceCollection services)
    {
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        return services;
    }

    /// <summary>
    /// Adds rate limiting configuration with IP-based partitioning.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddFixedWindowRateLimiter(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings =
            configuration.GetSection("RateLimiter").Get<RateLimiterConfiguration>()
            ?? new RateLimiterConfiguration();

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext =>
                {
                    var partitionKey = HttpContextUtilities.ExtractIpAddress(httpContext);

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = settings.PermitLimit,
                            Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = settings.QueueLimit
                        }
                    );
                }
            );

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
