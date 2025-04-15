using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Configurations;

/// <summary>
/// Provides centralized configuration methods for Swagger/OpenAPI docs.
/// Includes XML comments path resolution and security (JWT Bearer) setup.
/// </summary>
public static class SwaggerGenDefaults
{
    /// <summary>
    /// Resolves the path to the XML comments file generated from code
    /// documentation.
    /// This is used to enrich the Swagger UI with method summaries and remarks.
    /// </summary>
    /// <returns>Full file path to the XML documentation file.</returns>
    public static string ConfigureXmlCommentsFilePath()
    {
        return Path.Combine(
            AppContext.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
        );
    }

    /// <summary>
    /// Configures the OpenAPI security definition for JWT Bearer authentication.
    /// This will show the padlock icon in Swagger UI and allow users to
    /// authenticate.
    /// </summary>
    /// <returns>An <see cref="OpenApiSecurityScheme"/> describing the Bearer token format.</returns>
    public static OpenApiSecurityScheme ConfigureSecurityDefinition()
    {
        return new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token below. Example: Bearer {token}"
        };
    }

    /// <summary>
    /// Adds a global security requirement to the Swagger spec that applies
    /// the Bearer definition.
    /// Routes decorated with [Authorize] will be marked as protected in the UI.
    /// </summary>
    /// <returns>An <see cref="OpenApiSecurityRequirement"/> referencing the Bearer definition.</returns>
    public static OpenApiSecurityRequirement ConfigureSecurityRequirement()
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        };
    }
}
