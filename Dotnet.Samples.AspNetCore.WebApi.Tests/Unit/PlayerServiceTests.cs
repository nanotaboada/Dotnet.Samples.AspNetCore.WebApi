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
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        await service.CreateAsync(It.IsAny<PlayerRequestModel>());

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
        var players = PlayerFakes.GetStarting11();
        var response = PlayerFakes.CreateStarting11ResponseModels();
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        mapper
            .Setup(mapper => mapper.Map<List<PlayerResponseModel>>(It.IsAny<List<Player>>()))
            .Returns(response);
        var value = It.IsAny<object>();

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
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(PlayerFakes.GetStarting11());
        mapper
            .Setup(mapper => mapper.Map<List<PlayerResponseModel>>(It.IsAny<List<Player>>()))
            .Returns(PlayerFakes.CreateStarting11ResponseModels());
        var value = It.IsAny<object>();

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
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Player?)null);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(It.IsAny<long>());

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<long>()), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenRetrieveByIdAsync_WhenRepositoryFindByIdAsyncReturnsPlayer_TheResultShouldBePlayer()
    {
        // Arrange
        var id = 9;
        var player = PlayerFakes.GetOneExistingById(id);
        var response = PlayerFakes.CreateResponseModelForOneExistingById(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(player);
        mapper
            .Setup(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()))
            .Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(It.IsAny<long>());

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
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync((Player?)null);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveBySquadNumberAsync(It.IsAny<int>());

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
        var id = 9;
        var player = PlayerFakes.GetOneExistingById(id);
        var response = PlayerFakes.CreateResponseModelForOneExistingById(id);
        var (repository, logger, memoryCache, mapper) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync(player);
        mapper
            .Setup(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()))
            .Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object
        );

        // Act
        var result = await service.RetrieveBySquadNumberAsync(It.IsAny<int>());

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
        var id = 9;
        var player = PlayerFakes.GetOneExistingById(id);
        var request = PlayerFakes.CreateRequestModelForOneExistingById(id);
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
        var id = 9;
        var player = PlayerFakes.GetOneExistingById(id);
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
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Exactly(1));
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
