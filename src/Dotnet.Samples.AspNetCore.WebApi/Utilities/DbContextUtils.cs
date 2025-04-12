using Dotnet.Samples.AspNetCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities;

public static class DbContextUtils
{
    /// <summary>
    /// Seeds the database with initial data if empty.
    /// </summary>
    /// <remarks>
    /// This method checks if the database is empty and seeds it with initial data.
    /// It is designed to be used with UseAsyncSeeding.
    /// </remarks>
    /// <param name="context">The database context to seed.</param>
    /// <param name="_">Unused parameter, required by the UseAsyncSeeding API.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous seeding operation.</returns>
    public static Task SeedAsync(DbContext context, bool _, CancellationToken cancellationToken)
    {
        if (context is not PlayerDbContext playerDbContext)
        {
            throw new ArgumentException(
                $"Expected context of type {nameof(PlayerDbContext)}, but got {context.GetType().Name}",
                nameof(context)
            );
        }
        return SeedPlayersAsync(playerDbContext, cancellationToken);
    }

    private static async Task SeedPlayersAsync(
        PlayerDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        if (!await dbContext.Players.AnyAsync(cancellationToken))
        {
            await dbContext.Players.AddRangeAsync(PlayerData.MakeStarting11(), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
