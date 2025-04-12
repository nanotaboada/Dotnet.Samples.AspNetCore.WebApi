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
    public async Task GivenCreateAsync_WhenRepositoryAddAsync_ThenAddsPlayerToRepositoryAndRemovesCache()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var response = PlayerFakes.MakeResponseModelForCreate();
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(request)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        await service.CreateAsync(request);

        // Assert
        repository.Verify(repository => repository.AddAsync(It.IsAny<Player>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
        mapper.Verify(
            mapper => mapper.Map<PlayerResponseModel>(It.IsAny<PlayerRequestModel>()),
            Times.Once
        );
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveAsync_WhenRepositoryGetAllAsyncReturnsPlayers_ThenCacheCreateEntryAndResultShouldBeListOfPlayers()
    {
        // Arrange
        var value = It.IsAny<object>();
        var players = PlayerFakes.MakeStarting11();
        var response = PlayerFakes.MakeResponseModelsForRetrieve();
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        mapper.Setup(mapper => mapper.Map<List<PlayerResponseModel>>(players)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveAsync();

        // Assert
        repository.Verify(repository => repository.GetAllAsync(), Times.Once);
        memoryCache.Verify(cache => cache.TryGetValue(It.IsAny<object>(), out value), Times.Once);
        memoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once);
        mapper.Verify(
            mapper => mapper.Map<List<PlayerResponseModel>>(It.IsAny<List<Player>>()),
            Times.Once
        );
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveAsync_WhenExecutedForTheSecondTime_ThenSecondExecutionTimeShouldBeLessThanFirst()
    {
        // Arrange
        var value = It.IsAny<object>();
        var players = PlayerFakes.MakeStarting11();
        var response = PlayerFakes.MakeResponseModelsForRetrieve();
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        mapper.Setup(mapper => mapper.Map<List<PlayerResponseModel>>(players)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var first = await ExecutionTimeAsync(() => service.RetrieveAsync());
        var second = await ExecutionTimeAsync(() => service.RetrieveAsync());

        // Assert
        memoryCache.Verify(
            cache => cache.TryGetValue(It.IsAny<object>(), out value),
            Times.Exactly(2) // first + second
        );
        memoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once); // first only
        repository.Verify(repository => repository.GetAllAsync(), Times.Once); // first only
        mapper.Verify(
            mapper => mapper.Map<List<PlayerResponseModel>>(It.IsAny<List<Player>>()),
            Times.Once // first only
        );
        second.Should().BeLessThan(first);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveByIdAsync_WhenRepositoryFindByIdAsyncReturnsNull_TheResultShouldBeNull()
    {
        // Arrange
        var id = 999;
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Never);
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveByIdAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_TheResultShouldBePlayer()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.MakeFromStarting11ById(id);
        var response = PlayerFakes.MakeResponseModelForRetrieve(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(player);
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(player)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Once);
        result.Should().BeOfType<PlayerResponseModel>();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveBySquadNumberAsync_WhenRepositoryFindBySquadNumberAsyncReturnsNull_ThenResultShouldBeNull()
    {
        // Arrange
        var squadNumber = 999;
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveBySquadNumberAsync(squadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Never);
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveBySquadNumberAsync_WhenRepositoryFindBySquadNumberAsyncReturnsPlayer_ThenResultShouldBePlayer()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.MakeFromStarting11ById(id);
        var squadNumber = player.SquadNumber;
        var response = PlayerFakes.MakeResponseModelForRetrieve(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(player);
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(player)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveBySquadNumberAsync(squadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Once);
        result.Should().BeOfType<PlayerResponseModel>();
        result.Should().BeEquivalentTo(response);
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenUpdateAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_ThenRepositoryUpdateAsyncAndCacheRemove()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.MakeFromStarting11ById(id);
        var request = PlayerFakes.MakeRequestModelForUpdate(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        await service.UpdateAsync(request);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        repository.Verify(repository => repository.UpdateAsync(It.IsAny<Player>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
        mapper.Verify(
            mapper => mapper.Map(It.IsAny<PlayerRequestModel>(), It.IsAny<Player>()),
            Times.Once
        );
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_ThenRepositoryDeleteAsyncAndCacheRemove()
    {
        // Arrange
        var id = 2;
        var player = PlayerFakes.MakeFromStarting11ById(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        await service.DeleteAsync(It.IsAny<long>());

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        repository.Verify(repository => repository.RemoveAsync(It.IsAny<long>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
    }

    private static async Task<long> ExecutionTimeAsync(Func<Task> awaitable)
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
