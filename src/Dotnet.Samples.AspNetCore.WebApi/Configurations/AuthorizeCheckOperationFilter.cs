using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dotnet.Samples.AspNetCore.WebApi.Configurations
{
    /// <summary>
    /// Adds the Bearer security requirement only to operations that have [Authorize] attributes.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the Bearer security requirement to Swagger operations with [Authorize] attributes.
        /// </summary>
        /// <param name="operation">The OpenAPI operation to modify.</param>
        /// <param name="context">The operation filter context containing method metadata.</param>
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
            // In Microsoft.OpenApi 2.x, use OpenApiSecuritySchemeReference instead of OpenApiSecurityScheme with nested Reference
            if (operation is OpenApiOperation openApiOperation)
            {
                openApiOperation.Security ??= new List<OpenApiSecurityRequirement>();
                openApiOperation.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecuritySchemeReference("Bearer", null), new List<string>() }
                    }
                );
            }
        }
    }
}
