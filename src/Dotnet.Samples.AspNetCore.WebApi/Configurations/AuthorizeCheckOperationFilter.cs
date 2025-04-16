using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dotnet.Samples.AspNetCore.WebApi.Configurations
{
    /// <summary>
    /// Adds the Bearer security requirement only to operations that have [Authorize] attributes.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if [Authorize] is applied at the method or class level
            var hasAuthorize =
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                || context
                    .MethodInfo.DeclaringType?.GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any() == true;

            // If there's no [Authorize] attribute, skip adding the security requirement
            if (!hasAuthorize)
                return;

            // Add security requirement (shows the lock icon)
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(
                new OpenApiSecurityRequirement
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
                }
            );
        }
    }
}
