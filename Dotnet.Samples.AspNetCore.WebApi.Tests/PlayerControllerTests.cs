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
        var response = await controller.PostAsync(It.IsAny<Player>()) as BadRequest;

        // Assert
        response.Should().NotBeNull().And.BeOfType<BadRequest>();
        response?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "PostAsync")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe409Conflict()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(10);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.PostAsync(player) as Conflict;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<Conflict>();
        response?.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    [Trait("Category", "PostAsync")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe201Created()
    {
        // Arrange
        var player = PlayerData.CreateOneNew();

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
        var response = await controller.PostAsync(player) as Created<Player>;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.CreateAsync(It.IsAny<Player>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<Created<Player>>();
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
        var players = PlayerData.CreateStarting11();
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.GetAsync() as Ok<List<Player>>;

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<Ok<List<Player>>>();
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Value.Should().NotBeNull().And.BeOfType<List<Player>>();
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
        var response = await controller.GetAsync() as NotFound;

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NotFound>();
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
        var response = await controller.GetByIdAsync(It.IsAny<long>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NotFound>();
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "GetByIdAsync")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(10);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.GetByIdAsync(It.IsAny<long>()) as Ok<Player>;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<Ok<Player>>();
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Value.Should().NotBeNull().And.BeOfType<Player>();
        response?.Value.Should().BeEquivalentTo(player);
    }

    [Fact]
    [Trait("Category", "GetBySquadNumberAsync")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync(null as Player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.GetBySquadNumberAsync(It.IsAny<int>()) as NotFound;

        // Assert
        service.Verify(
            service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()),
            Times.Exactly(1)
        );
        response.Should().NotBeNull().And.BeOfType<NotFound>();
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "GetBySquadNumberAsync")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(10);
        var service = new Mock<IPlayerService>();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync(player);
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.GetBySquadNumberAsync(It.IsAny<int>()) as Ok<Player>;

        // Assert
        service.Verify(
            service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()),
            Times.Exactly(1)
        );
        response.Should().NotBeNull().And.BeOfType<Ok<Player>>();
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Value.Should().NotBeNull().And.BeOfType<Player>();
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
        var response =
            await controller.PutAsync(It.IsAny<long>(), It.IsAny<Player>()) as BadRequest;

        // Assert
        response.Should().NotBeNull().And.BeOfType<BadRequest>();
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
        var response = await controller.PutAsync(It.IsAny<long>(), It.IsAny<Player>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NotFound>();
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "PutAsync")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var id = 10;
        var player = PlayerData.CreateOneByIdFromStarting11(id);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        service.Setup(service => service.UpdateAsync(It.IsAny<Player>()));
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.PutAsync(id, player) as NoContent;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.UpdateAsync(It.IsAny<Player>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NoContent>();
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
        var response = await controller.DeleteAsync(It.IsAny<long>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NotFound>();
        response?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "DeleteAsync")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var player = PlayerData.CreateOneByIdFromStarting11(10);
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        service.Setup(service => service.DeleteAsync(It.IsAny<long>()));
        var logger = PlayerMocks.LoggerMock<PlayersController>();

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.DeleteAsync(It.IsAny<long>()) as NoContent;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.DeleteAsync(It.IsAny<long>()), Times.Exactly(1));
        response.Should().NotBeNull().And.BeOfType<NoContent>();
        response?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
