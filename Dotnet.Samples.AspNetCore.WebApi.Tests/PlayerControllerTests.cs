using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests;

public class PlayerControllerTests
{
    /* -------------------------------------------------------------------------
     * HTTP POST
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "PostAsync")]
    public async Task GivenPostAsync_WhenModelStateIsInvalid_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var service = new Mock<IPlayerService>();
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);
        controller.ModelState.Merge(PlayerStubs.CreateModelError("FirstName", "Required"));

        // Act
        var result = await controller.PostAsync(It.IsAny<Player>());

        // Assert
        result.Should().BeOfType<BadRequest>();
        var response = result as BadRequest;
        response?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "PostAsync")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe409Conflict()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneById(10);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.PostAsync(player);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<Conflict>();
        var response = result as Conflict;
        response?.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    [Trait("Category", "PostAsync")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe201Created()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneNew();

        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Player);
        service.Setup(service => service.CreateAsync(It.IsAny<Player>()));
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object)
        {
            Url = PlayerMocks.UrlHelperMock().Object,
        };

        // Act
        var result = await controller.PostAsync(player);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.CreateAsync(It.IsAny<Player>()), Times.Exactly(1));
        result.Should().BeOfType<Created<Player>>();
        var response = result as Created<Player>;
        response?.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsListOfPlayers_ThenResponseShouldBeEquivalentToListOfPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithStarting11();
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

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
        var logger = PlayerMocks.LoggerMock<PlayersController>();

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
        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

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
        var logger = PlayerMocks.LoggerMock<PlayersController>();

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

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "PutAsync")]
    public async Task GivenPutAsync_WhenModelStateIsInvalid_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var service = new Mock<IPlayerService>();
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);
        controller.ModelState.Merge(PlayerStubs.CreateModelError("FirstName", "Required"));

        // Act
        var result = await controller.PutAsync(It.IsAny<long>(), It.IsAny<Player>());

        // Assert
        result.Should().BeOfType<BadRequest>();
        var response = result as BadRequest;
        response?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "PutAsync")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.PutAsync(It.IsAny<long>(), It.IsAny<Player>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<NotFound>();
        var response = result as NotFound;
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "PutAsync")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var id = 10;
        var player = PlayerDataBuilder.SeedOneById(id);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        service.Setup(service => service.UpdateAsync(It.IsAny<Player>()));
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.PutAsync(id, player);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.UpdateAsync(It.IsAny<Player>()), Times.Exactly(1));
        result.Should().BeOfType<NoContent>();
        var response = result as NoContent;
        response?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "DeleteAsync")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<long>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<NotFound>();
        var response = result as NotFound;
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "DeleteAsync")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var player = PlayerDataBuilder.SeedOneById(10);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        service.Setup(service => service.DeleteAsync(It.IsAny<long>()));
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<long>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.DeleteAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().BeOfType<NoContent>();
        var response = result as NoContent;
        response?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
