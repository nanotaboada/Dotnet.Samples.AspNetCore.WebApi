namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Simple extension method to populate the database with an initial set of data.
    /// </summary>
    public static void SeedDbContext(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<PlayerDbContext>();

        if (dbContext != null)
        {
            // https://learn.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
            dbContext.Database.EnsureCreated();

            if (!dbContext.Players.Any())
            {
                dbContext.Players.AddRange(PlayerData.CreateStarting11());
            }
        }
    }
}
