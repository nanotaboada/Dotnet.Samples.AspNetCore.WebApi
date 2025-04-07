using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerControllerTests : IDisposable
{
    private bool _disposed;

    public PlayerControllerTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    }

    /* -------------------------------------------------------------------------
     * HTTP POST
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenModelStateIsInvalid_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();

        var controller = new PlayerController(service.Object, logger.Object);
        controller.ModelState.Merge(PlayerStubs.CreateModelError("SquadNumber", "Required"));

        // Act
        var result = await controller.PostAsync(It.IsAny<PlayerRequestModel>()) as BadRequest;

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequest>();
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe409Conflict()
    {
        // Arrange
        var id = 10;
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(PlayerFakes.CreateResponseModelForOneExistingById(id));
        var payload = PlayerFakes.CreateRequestModelForOneExistingById(id);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.PostAsync(payload) as Conflict;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<Conflict>();
        result?.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe201Created()
    {
        // Arrange
        var request = PlayerFakes.CreateRequestModelForOneNew();
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as PlayerResponseModel);
        service.Setup(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()));

        var controller = new PlayerController(service.Object, logger.Object)
        {
            Url = PlayerMocks.SetupUrlHelperMock().Object,
        };

        // Act
        var result = await controller.PostAsync(request) as Created<PlayerRequestModel>;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(
            service => service.CreateAsync(It.IsAny<PlayerRequestModel>()),
            Times.Exactly(1)
        );
        result.Should().NotBeNull().And.BeOfType<Created<PlayerRequestModel>>();
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.Should().BeEquivalentTo(request); // Request not mapped to Response
        result?.Location.Should().Be($"/players/{request.Id}");
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsListOfPlayers_ThenResponseShouldBeEquivalentToListOfPlayers()
    {
        // Arrange
        var players = PlayerFakes.CreateStarting11ResponseModels();
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.GetAsync() as Ok<List<PlayerResponseModel>>;

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<Ok<List<PlayerResponseModel>>>();
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result?.Value.Should().NotBeNull().And.BeOfType<List<PlayerResponseModel>>();
        result?.Value.Should().BeEquivalentTo(players);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsEmptyList_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync([]);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.GetAsync() as NotFound;

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<NotFound>();
        result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.GetByIdAsync(It.IsAny<long>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<NotFound>();
        result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var player = PlayerFakes.CreateResponseModelForOneExistingById(10);
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.GetByIdAsync(It.IsAny<long>()) as Ok<PlayerResponseModel>;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result?.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
        result?.Value.Should().BeEquivalentTo(player);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.GetBySquadNumberAsync(It.IsAny<int>()) as NotFound;

        // Assert
        service.Verify(
            service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()),
            Times.Exactly(1)
        );
        result.Should().NotBeNull().And.BeOfType<NotFound>();
        result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var player = PlayerFakes.CreateResponseModelForOneExistingById(10);
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()))
            .ReturnsAsync(player);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result =
            await controller.GetBySquadNumberAsync(It.IsAny<int>()) as Ok<PlayerResponseModel>;

        // Assert
        service.Verify(
            service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()),
            Times.Exactly(1)
        );
        result.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result?.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
        result?.Value.Should().BeEquivalentTo(player);
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenModelStateIsInvalid_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();

        var controller = new PlayerController(service.Object, logger.Object);
        controller.ModelState.Merge(PlayerStubs.CreateModelError("SquadNumber", "Required"));

        // Act
        var result =
            await controller.PutAsync(It.IsAny<long>(), It.IsAny<PlayerRequestModel>())
            as BadRequest;

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequest>();
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result =
            await controller.PutAsync(It.IsAny<long>(), It.IsAny<PlayerRequestModel>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<NotFound>();
        result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var id = 10;
        var player = PlayerFakes.CreateRequestModelForOneExistingById(id);
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(PlayerFakes.CreateResponseModelForOneExistingById(id));
        service.Setup(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()));

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.PutAsync(id, player) as NoContent;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(
            service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()),
            Times.Exactly(1)
        );
        result.Should().NotBeNull().And.BeOfType<NoContent>();
        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<long>()) as NotFound;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<NotFound>();
        result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var (service, logger) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(PlayerFakes.CreateResponseModelForOneExistingById(10));
        service.Setup(service => service.DeleteAsync(It.IsAny<long>()));

        var controller = new PlayerController(service.Object, logger.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<long>()) as NoContent;

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Exactly(1));
        service.Verify(service => service.DeleteAsync(It.IsAny<long>()), Times.Exactly(1));
        result.Should().NotBeNull().And.BeOfType<NoContent>();
        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
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
