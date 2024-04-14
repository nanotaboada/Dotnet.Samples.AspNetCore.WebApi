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
    private readonly DbContextOptions<PlayerContext> _dbContextOptions;
    private readonly PlayerContext _context;

    public PlayerServiceTests()
    {
        (_dbConnection, _dbContextOptions) = PlayerStubs.CreateSqliteConnection();
        _context = PlayerStubs.CreateContext(_dbContextOptions);
        PlayerStubs.CreateTable(_context);
        PlayerStubs.SeedContext(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieveAsync_WhenInvoked_ThenShouldReturnAllPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithDeserializedJson();
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(_context, logger, memoryCache);

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
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(players);

        var service = new PlayerService(_context, logger, memoryCache);

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
        var logger = PlayerMocks.LoggerMock<PlayerService>();
        var memoryCache = PlayerMocks.MemoryCacheMock(It.IsAny<object>());

        var service = new PlayerService(_context, logger, memoryCache);

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
