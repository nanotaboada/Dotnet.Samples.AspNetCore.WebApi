using Dotnet.Samples.AspNetCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Async extension method to populate the database with initial data
    /// </summary>
    public static async Task SeedDbContextAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var dbContext = services.GetRequiredService<PlayerDbContext>();

        try
        {
            await dbContext.Database.EnsureCreatedAsync();

            if (!await dbContext.Players.AnyAsync())
            {
                await dbContext.Players.AddRangeAsync(PlayerData.CreateStarting11());
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Successfully seeded database with initial data.");
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while seeding the database");
            throw new InvalidOperationException(
                "An error occurred while seeding the database",
                exception
            );
        }
    }
}
