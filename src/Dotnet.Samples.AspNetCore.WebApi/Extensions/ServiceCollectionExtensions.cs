using System.Threading.RateLimiting;
using Dotnet.Samples.AspNetCore.WebApi.Configurations;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Mappings;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Dotnet.Samples.AspNetCore.WebApi.Extensions;

/// <summary>
/// Extension methods for WebApplicationBuilder to encapsulate service configuration.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DbContextPool with SQLite configuration for PlayerDbContext.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="environment">The web host environment.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddDbContextPoolWithSqlite(
        this IServiceCollection services,
        IWebHostEnvironment environment
    )
    {
        services.AddDbContextPool<PlayerDbContext>(options =>
        {
            var dataSource = Path.Combine(
                AppContext.BaseDirectory,
                "storage",
                "players-sqlite3.db"
            );
            options.UseSqlite($"Data Source={dataSource}");

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.LogTo(Log.Logger.Information, LogLevel.Information);
            }
        });

        return services;
    }

    /// <summary>
    /// Adds a default CORS policy that allows any origin, method, and header.
    /// <br />
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/security/cors"/>
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection for method chaining.</returns>
    public static IServiceCollection AddCorsDefaultPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

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
            options.SwaggerDoc("v1", configuration.GetSection("OpenApiInfo").Get<OpenApiInfo>());
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
        services.AddAutoMapper(typeof(PlayerMappingProfile));
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
}
