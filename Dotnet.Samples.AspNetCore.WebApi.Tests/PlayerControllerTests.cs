using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests;

public class PlayerControllerTests
{
    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsListOfPlayers_ThenResponseShouldBeEquivalentToListOfPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithStarting11();

        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);
        var logger = new Mock<ILogger<PlayersController>>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        result.Should().BeOfType<Ok<List<Player>>>();
        var response = result as Ok<List<Player>>;
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Value.Should().BeOfType<List<Player>>();
        response?.Value.Should().BeEquivalentTo(players);
    }

    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsEmptyList_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var players = new List<Player>(); // Count = 0

        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);
        var logger = new Mock<ILogger<PlayersController>>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        result.Should().BeOfType<NotFound>();
        var response = result as NotFound;
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "GetByIdAsync")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var player = (Player?)null;

        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        var logger = new Mock<ILogger<PlayersController>>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.GetByIdAsync(It.IsAny<long>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<NotFound>();
        var response = result as NotFound;
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "GetByIdAsync")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneById(10);

        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        var logger = new Mock<ILogger<PlayersController>>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.GetByIdAsync(It.IsAny<long>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<Ok<Player>>();
        var response = result as Ok<Player>;
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Value.Should().BeOfType<Player>();
        response?.Value.Should().BeEquivalentTo(player);
    }
}
