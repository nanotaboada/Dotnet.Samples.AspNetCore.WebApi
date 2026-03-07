using System.Diagnostics;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerServiceTests
{
    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CreateAsync_WhenCalled_AddsPlayerAndRemovesCache()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var response = PlayerFakes.MakeResponseModelForCreate();
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(request)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
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
    public async Task RetrieveAsync_CacheMiss_QueriesRepositoryAndCachesResult()
    {
        // Arrange
        var value = It.IsAny<object>();
        var players = PlayerFakes.MakeStarting11();
        var response = PlayerFakes.MakeResponseModelsForRetrieve();
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        mapper.Setup(mapper => mapper.Map<List<PlayerResponseModel>>(players)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
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
    public async Task RetrieveAsync_CacheHit_ReturnsBeforeQueryingRepository()
    {
        // Arrange
        var value = It.IsAny<object>();
        var players = PlayerFakes.MakeStarting11();
        var response = PlayerFakes.MakeResponseModelsForRetrieve();
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(players);
        mapper.Setup(mapper => mapper.Map<List<PlayerResponseModel>>(players)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
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
    public async Task RetrieveByIdAsync_PlayerNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Never);
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetrieveByIdAsync_PlayerFound_ReturnsMappedResponseModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var squadNumber = 10;
        var player = PlayerFakes.MakeFromStarting11(squadNumber);
        var response = PlayerFakes.MakeResponseModelForRetrieve(squadNumber);
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(player);
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(player)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        var result = await service.RetrieveByIdAsync(id);

        // Assert
        repository.Verify(repository => repository.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        mapper.Verify(mapper => mapper.Map<PlayerResponseModel>(It.IsAny<Player>()), Times.Once);
        result.Should().BeOfType<PlayerResponseModel>();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetrieveBySquadNumberAsync_PlayerNotFound_ReturnsNull()
    {
        // Arrange
        var squadNumber = 999;
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
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
    public async Task RetrieveBySquadNumberAsync_PlayerFound_ReturnsMappedResponseModel()
    {
        // Arrange
        var squadNumber = 10;
        var player = PlayerFakes.MakeFromStarting11(squadNumber);
        var response = PlayerFakes.MakeResponseModelForRetrieve(squadNumber);
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(player);
        mapper.Setup(mapper => mapper.Map<PlayerResponseModel>(player)).Returns(response);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
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
    public async Task UpdateAsync_PlayerFound_UpdatesRepositoryAndRemovesCache()
    {
        // Arrange
        var squadNumber = 23;
        var player = PlayerFakes.MakeFromStarting11(squadNumber);
        var request = PlayerFakes.MakeRequestModelForUpdate(squadNumber);
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        await service.UpdateAsync(request);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        repository.Verify(repository => repository.UpdateAsync(It.IsAny<Player>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
        mapper.Verify(
            mapper => mapper.Map(It.IsAny<PlayerRequestModel>(), It.IsAny<Player>()),
            Times.Once
        );
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task UpdateAsync_PlayerNotFound_DoesNotUpdateRepository()
    {
        // Arrange
        var squadNumber = 999;
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.SquadNumber = squadNumber;
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        await service.UpdateAsync(request);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        repository.Verify(repository => repository.UpdateAsync(It.IsAny<Player>()), Times.Never);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Never);
        mapper.Verify(
            mapper => mapper.Map(It.IsAny<PlayerRequestModel>(), It.IsAny<Player>()),
            Times.Never
        );
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task DeleteAsync_PlayerFound_RemovesFromRepositoryAndRemovesCache()
    {
        // Arrange
        var squadNumber = 26;
        var player = PlayerFakes.MakeFromStarting11(squadNumber);
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        await service.DeleteAsync(squadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        repository.Verify(repository => repository.RemoveAsync(It.IsAny<Guid>()), Times.Once);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task DeleteAsync_PlayerNotFound_DoesNotRemoveFromRepository()
    {
        // Arrange
        var squadNumber = 999;
        var (repository, logger, memoryCache, mapper, environment) = PlayerMocks.InitServiceMocks();
        repository
            .Setup(repository => repository.FindBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as Player);

        var service = new PlayerService(
            repository.Object,
            logger.Object,
            memoryCache.Object,
            mapper.Object,
            environment.Object
        );

        // Act
        await service.DeleteAsync(squadNumber);

        // Assert
        repository.Verify(
            repository => repository.FindBySquadNumberAsync(It.IsAny<int>()),
            Times.Once
        );
        repository.Verify(repository => repository.RemoveAsync(It.IsAny<Guid>()), Times.Never);
        memoryCache.Verify(cache => cache.Remove(It.IsAny<object>()), Times.Never);
    }

    private static async Task<long> ExecutionTimeAsync(Func<Task> awaitable)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        await awaitable();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}
