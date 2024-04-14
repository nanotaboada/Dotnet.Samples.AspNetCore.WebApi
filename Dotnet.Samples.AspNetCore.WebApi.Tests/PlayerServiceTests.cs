using System.Data.Common;
using System.Diagnostics;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests;

public class PlayerServiceTests : IDisposable
{
    private readonly DbConnection dbConnection;
    private readonly DbContextOptions<PlayerContext> dbContextOptions;
    private readonly PlayerContext context;

    public PlayerServiceTests()
    {
        (dbConnection, dbContextOptions) = PlayerDatabaseBuilder.BuildDatabase();
        context = PlayerContextBuilder.CreatePlayerContext(dbContextOptions);

        PlayerDatabaseBuilder.CreateDatabase(context);
        PlayerDatabaseBuilder.Seed(context);
    }

    public void Dispose()
    {
        context.Dispose();
        dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveAsync_WhenInvoked_ThenShouldReturnAllPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithDeserializedJson();
        var logger = PlayerMocksBuilder.CreateLoggerMock<PlayerService>();
        var memoryCache = PlayerMocksBuilder.CreateMemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(context, logger, memoryCache);

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
        var players = PlayerDataBuilder.SeedWithDeserializedJson();
        var logger = PlayerMocksBuilder.CreateLoggerMock<PlayerService>();
        var memoryCache = PlayerMocksBuilder.CreateMemoryCacheMock(players);

        var service = new PlayerService(context, logger, memoryCache);

        // Act
        var first = await ExecutionTimeAsync(() => service.RetrieveAsync());
        var second = await ExecutionTimeAsync(() => service.RetrieveAsync());

        // Assert
        second.Should().BeLessThan(first);
    }

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveByIdAsync_WhenInvokedWithPlayerId_ThenShouldReturnThePlayer()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneById(10);
        var logger = PlayerMocksBuilder.CreateLoggerMock<PlayerService>();
        var memoryCache = PlayerMocksBuilder.CreateMemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(context, logger, memoryCache);

        // Act
        var result = await service.RetrieveByIdAsync(10);

        // Assert
        result.Should().BeOfType<Player>();
        result.Should().BeEquivalentTo(player);
    }

    private async Task<long> ExecutionTimeAsync(Func<Task> awaitable)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        await awaitable();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}
