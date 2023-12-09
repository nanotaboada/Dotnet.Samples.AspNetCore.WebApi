using System.Diagnostics;
using Dotnet.AspNetCore.Samples.WebApi.Data;
using Dotnet.AspNetCore.Samples.WebApi.Models;
using Dotnet.AspNetCore.Samples.WebApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Dotnet.AspNetCore.Samples.WebApi.Tests;

public class PlayerServiceTests
{
    [Fact]
    [Trait("Category", "Retrieve")]
    public async Task GivenRetrieve_WhenInvokedTwice_ThenSecondExecutionTimeShouldBeLessThanFirst()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithStarting11().ToList();
        var context = new Mock<PlayerContext>();
        context.Setup(context => context.Players).ReturnsDbSet(players);
        var logger = new Mock<ILogger<PlayerService>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var service = new PlayerService(context.Object, logger.Object, memoryCache);

        // Act
        var first = await ExecutionTime(() => service.Retrieve());
        var second = await ExecutionTime(() => service.Retrieve());

        // Assert
        second.Should().BeLessThan(first);
    }

    public async Task<long> ExecutionTime(Func<Task> awaitable)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await awaitable();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}