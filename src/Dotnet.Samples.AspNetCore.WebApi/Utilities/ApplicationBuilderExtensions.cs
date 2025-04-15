using Dotnet.Samples.AspNetCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities
{
    /// <summary>
    /// Provides extension methods for <see cref="IApplicationBuilder"/> to
    /// simplify database seeding operations.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Ensures the SQLite database file exists and contains tables.
        /// If the database is newly created, it seeds it with initial data.
        /// Does not apply or validate migrations.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        public static async Task InitData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var dbContext = services.GetRequiredService<PlayerDbContext>();
            try
            {
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Database successfully migrated.");

                if (!await dbContext.Players.AnyAsync())
                {
                    DbContextUtils.Seed(dbContext);
                    logger.LogInformation("DbContext successfully seeded.");
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while initializing the database.");
                throw new InvalidOperationException(
                    "Failed to initialize the database.",
                    exception
                );
            }
        }
    }
}
