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
    private readonly DbConnection _dbConnection;
    private readonly DbContextOptions<PlayerDbContext> _dbContextOptions;
    private readonly PlayerDbContext _dbContext;

    public PlayerServiceTests()
    {
        (_dbConnection, _dbContextOptions) = PlayerStubs.CreateSqliteConnection();
        _dbContext = PlayerStubs.CreateDbContext(_dbContextOptions);
        PlayerStubs.CreateTable(_dbContext);
        PlayerStubs.SeedDbContext(_dbContext);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "CreateAsync")]
    public async Task GivenCreateAsync_WhenInvokedWithPlayer_ThenShouldAddPlayerToContextAndRemovePlayersCache()
    {
        // Arrange
        var player = PlayerData.CreateOneNew();
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        await service.CreateAsync(player);
        var result = await _dbContext.Players.FindAsync(player.Id);

        // Assert
        result.Should().NotBeNull();
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Exactly(1));
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "RetrieveAsync")]
    public async Task GivenRetrieveAsync_WhenInvoked_ThenShouldReturnAllPlayersAndCreatePlayersCache()
    {
        // Arrange
        var players = PlayerData.CreateStarting11();
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());
        var value = It.IsAny<object>();

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveAsync();

        // Assert
        memoryCache.Verify(
            cache => cache.TryGetValue(It.IsAny<object>(), out value),
            Times.Exactly(1)
        );
        memoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Exactly(1));
        result.Should().BeEquivalentTo(players);
    }

    [Fact]
    [Trait("Category", "RetrieveAsync")]
    public async Task GivenRetrieveAsync_WhenInvokedTwice_ThenSecondExecutionTimeShouldBeLessThanFirst()
    {
        // Arrange
        var players = PlayerData.CreateStarting11();
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(players);
        var value = It.IsAny<object>();

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        var first = await ExecutionTimeAsync(() => service.RetrieveAsync());
        var second = await ExecutionTimeAsync(() => service.RetrieveAsync());

        // Assert
        memoryCache.Verify(
            cache => cache.TryGetValue(It.IsAny<object>(), out value),
            Times.Exactly(2) // first + second
        );
        second.Should().BeLessThan(first);
    }

    [Fact]
    [Trait("Category", "RetrieveByIdAsync")]
    public async Task GivenRetrieveByIdAsync_WhenInvokedWithPlayerId_ThenShouldReturnThePlayer()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(10);
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveByIdAsync(10);

        // Assert
        result.Should().BeOfType<Player>();
        result.Should().BeEquivalentTo(player);
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "UpdateAsync")]
    public async Task GivenUpdateAsync_WhenInvokedWithPlayer_ThenShouldModifyPlayerInContextAndRemovePlayersCache()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(1);
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        player.FirstName = "Emiliano";
        player.MiddleName = "";
        await service.UpdateAsync(player);
        var result = await _dbContext.Players.FindAsync(player.Id);

        // Assert
        result!.FirstName.Should().Be(player.FirstName);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Exactly(1));
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "DeleteAsync")]
    public async Task GivenDeleteAsync_WhenInvokedWithPlayerId_ThenShouldRemovePlayerFromContextAndRemovePlayersCache()
    {
        // Arrange
        var player = PlayerData.CreateOneNew();
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());
        await _dbContext.AddAsync(player);
        await _dbContext.SaveChangesAsync();

        var service = new PlayerService(_dbContext, logger.Object, memoryCache.Object);

        // Act
        await service.DeleteAsync(player.Id);
        var result = await _dbContext.Players.FindAsync(player.Id);

        // Assert
        result.Should().BeNull();
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Exactly(1));
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
