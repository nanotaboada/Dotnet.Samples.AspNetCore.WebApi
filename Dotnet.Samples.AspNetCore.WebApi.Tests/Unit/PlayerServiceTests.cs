using System.Diagnostics;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerServiceTests : IDisposable
{
    private bool _disposed;

    public PlayerServiceTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    }

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenCreateAsync_WhenRepositoryAddAsync_ThenAddsPlayerToRepositoryAndRemovesMemoryCache()
    {
        // Arrange
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        await service.CreateAsync(It.IsAny<Player>());

        // Assert
        repository.Verify(repository => repository.AddAsync(It.IsAny<Player>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveAsync_WhenRepositoryGetAllAsyncReturnsPlayers_ThenCacheCreateEntryAndResultShouldBeListOfPlayers()
    {
        // Arrange
        var players = PlayerFakes.CreateStarting11();
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        var value = It.IsAny<object>();

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveAsync();

        // Assert
        repository.Verify(repository => repository.GetAllAsync(), Times.Once);
        memoryCache.Verify(cache => cache.TryGetValue(It.IsAny<object>(), out value), Times.Once);
        memoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once);
        result.Should().BeEquivalentTo(players);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveAsync_WhenExecutedForTheSecondTime_ThenSecondExecutionTimeShouldBeLessThanFirst()
    {
        // Arrange
        var players = PlayerFakes.CreateStarting11();
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks(cacheValue: players);
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        var value = It.IsAny<object>();

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

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
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveByIdAsync_WhenRepositoryFindByIdAsyncReturnsNull_TheResultShouldBeNull()
    {
        // Arrange
        var id = 999;
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync((Player?)null);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveByIdAsync(id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveByIdAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_TheResultShouldBePlayer()
    {
        // Arrange
        var player = PlayerFakes.CreateOneByIdFromStarting11(9);
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(player.Id)).ReturnsAsync(player);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveByIdAsync(player.Id);

        // Assert
        result.Should().BeOfType<Player>();
        result.Should().BeEquivalentTo(player);
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveBySquadNumberAsync_WhenRepositoryFindBySquadNumberAsyncReturnsNull_ThenResultShouldBeNull()
    {
        // Arrange
        var squadNumber = 999;
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync((Player?)null);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveBySquadNumberAsync(squadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveBySquadNumberAsync_WhenRepositoryFindBySquadNumberAsyncReturnsPlayer_ThenResultShouldBePlayer()
    {
        // Arrange
        var player = PlayerFakes.CreateOneByIdFromStarting11(9);
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(player.SquadNumber))
            .ReturnsAsync(player);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        var result = await service.RetrieveBySquadNumberAsync(player.SquadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        result.Should().BeOfType<Player>();
        result.Should().BeEquivalentTo(player);
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenUpdateAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_ThenRepositoryUpdateAsyncAndCacheRemove()
    {
        // Arrange
        var player = PlayerFakes.CreateOneByIdFromStarting11(9);
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(player.Id)).ReturnsAsync(player);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        await service.UpdateAsync(player);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        repository.Verify(repository => repository.UpdateAsync(player), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_ThenRepositoryDeleteAsyncAndCacheRemove()
    {
        // Arrange
        var player = PlayerFakes.CreateOneNew();
        var (repository, logger, memoryCache) = PlayerMocks.SetupServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(player.Id)).ReturnsAsync(player);

        var service = new PlayerService(repository.Object, logger.Object, memoryCache.Object);

        // Act
        await service.DeleteAsync(player.Id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        repository.Verify(repository => repository.RemoveAsync(It.IsAny<long>()), Times.Once);
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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
