using System.Data.Common;
using System.Diagnostics;
using Dotnet.AspNetCore.Samples.WebApi.Data;
using Dotnet.AspNetCore.Samples.WebApi.Models;
using Dotnet.AspNetCore.Samples.WebApi.Services;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.AspNetCore.Samples.WebApi.Tests;

public class PlayerServiceTests : IDisposable
{
    private readonly DbConnection dbConnection;
    private readonly DbContextOptions<PlayerContext> dbContextOptions;

    public PlayerServiceTests()
    {
        dbConnection = new SqliteConnection("Filename=:memory:");
        dbConnection.Open();

        dbContextOptions = new DbContextOptionsBuilder<PlayerContext>()
            .UseSqlite(dbConnection)
            .Options;

        using var context = new PlayerContext(dbContextOptions);

        if (context.Database.EnsureCreated())
        {
            using var dbCommand = context.Database.GetDbConnection().CreateCommand();

            dbCommand.CommandText = """
                CREATE TABLE IF NOT EXISTS "players"
                (
                    "id"	        INTEGER,
                    "firstName"	    TEXT NOT NULL,
                    "middleName"    TEXT,
                    "lastName"	    TEXT NOT NULL,
                    "dateOfBirth"	TEXT,
                    "squadNumber"	INTEGER NOT NULL,
                    "position"      TEXT NOT NULL,
                    "abbrPosition"  TEXT,
                    "team"          TEXT,
                    "league"    	TEXT,
                    "starting11"    BOOLEAN,
                                    PRIMARY KEY("id")
                );
            """;
            
            dbCommand.ExecuteNonQuery();
        }

        context.AddRange(PlayerDataBuilder.SeedWithDeserializedJson());
        context.SaveChanges();
    }

    PlayerContext CreatePlayerContext() => new PlayerContext(dbContextOptions);
    public void Dispose() => dbConnection.Dispose();

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveAsync_WhenInvoked_ThenShouldReturnAllPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithDeserializedJson();
        var context = CreatePlayerContext();
        var logger = new Mock<ILogger<PlayerService>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var service = new PlayerService(context, logger.Object, memoryCache);

        // Act
        var result = await service.RetrieveAsync();

        // Assert
        result.Should().BeEquivalentTo(players);
    }

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveAsync_WhenInvokedTwice_ThenSecondExecutionTimeShouldBeLessThanFirst()
    {
        // Arrange
        var context = CreatePlayerContext();
        var logger = new Mock<ILogger<PlayerService>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var service = new PlayerService(context, logger.Object, memoryCache);

        // Act
        var first = await ExecutionTimeAsync(() => service.RetrieveAsync());
        var second = await ExecutionTimeAsync(() => service.RetrieveAsync());

        // Assert
        second.Should().BeLessThan(first);
    }

    private async Task<long> ExecutionTimeAsync(Func<Task> awaitable)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await awaitable();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveByIdAsync_WhenInvokedWithPlayerId_ThenShouldReturnThePlayer()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneById(10);
        var context = CreatePlayerContext();
        var logger = new Mock<ILogger<PlayerService>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var service = new PlayerService(context, logger.Object, memoryCache);

        // Act
        var result = await service.RetrieveByIdAsync(10);

        // Assert
        result.Should().BeOfType<Player>();
        result.Should().BeEquivalentTo(player);
    }
}
