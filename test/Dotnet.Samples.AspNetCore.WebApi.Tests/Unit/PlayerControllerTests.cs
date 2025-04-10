using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using FluentValidation.Results;
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
    public async Task GivenPostAsync_WhenValidatorReturnsErrors_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var payload = PlayerFakes.CreateRequestModelForOneNew();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        validator
            .Setup(validator => validator.ValidateAsync(payload, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("SquadNumber", "SquadNumber must be greater than 0.")
                    }
                )
            );

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PostAsync(payload);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Never);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is BadRequest response)
        {
            response.Should().NotBeNull().And.BeOfType<BadRequest>();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe409Conflict()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.CreateResponseModelForOneExistingById(id);
        var payload = PlayerFakes.CreateRequestModelForOneExistingById(id);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveByIdAsync(It.IsAny<long>())).ReturnsAsync(player);
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PostAsync(payload);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is Conflict response)
        {
            response.Should().NotBeNull().And.BeOfType<Conflict>();
            response.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe201Created()
    {
        // Arrange
        var id = 1;
        var payload = PlayerFakes.CreateRequestModelForOneNew();
        var content = PlayerFakes.CreateResponseModelForOneNew();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(null as PlayerResponseModel);
        service
            .Setup(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()))
            .ReturnsAsync(content);
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object)
        {
            Url = PlayerMocks.SetupUrlHelperMock().Object,
        };

        // Act
        var result = await controller.PostAsync(payload);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Once);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is CreatedAtRoute<PlayerRequestModel> response)
        {
            response.Should().NotBeNull().And.BeOfType<Created<PlayerResponseModel>>();
            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Value.Should().BeEquivalentTo(content);
            response.RouteName.Should().Be("GetById");
            response.RouteValues.Should().NotBeNull().And.ContainKey("id");
        }
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
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(players);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Once);
        if (result is Ok<List<PlayerResponseModel>> response)
        {
            response.Should().NotBeNull().And.BeOfType<Ok<List<PlayerResponseModel>>>();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Value.Should().NotBeNull().And.BeOfType<List<PlayerResponseModel>>();
            response.Value.Should().BeEquivalentTo(players);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetAsync_WhenServiceRetrieveAsyncReturnsEmptyList_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync([]);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Once);
        if (result is NotFound response)
        {
            response.Should().NotBeNull().And.BeOfType<NotFound>();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetByIdAsync(It.IsAny<long>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        if (result is NotFound response)
        {
            response.Should().NotBeNull().And.BeOfType<NotFound>();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.CreateResponseModelForOneExistingById(id);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveByIdAsync(id)).ReturnsAsync(player);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetByIdAsync(id);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        if (result is Ok<PlayerResponseModel> response)
        {
            response.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
            response.Value.Should().BeEquivalentTo(player);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var squadNumber = 10;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetBySquadNumberAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        if (result is NotFound response)
        {
            response.Should().NotBeNull().And.BeOfType<NotFound>();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var squadNumber = 10;
        var player = PlayerFakes.CreateResponseModelForOneExistingById(1);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(player);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetBySquadNumberAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        if (result is Ok<PlayerResponseModel> response)
        {
            response.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
            response.Value.Should().BeEquivalentTo(player);
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenValidatorReturnsErrors_ThenResponseStatusCodeShouldBe400BadRequest()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.CreateRequestModelForOneExistingById(id);
        player.SquadNumber = -99; // Invalid Squad Number
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        validator
            .Setup(v => v.ValidateAsync(player, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("SquadNumber", "SquadNumber must be greater than 0.")
                    }
                )
            );

        // Act
        var result = await controller.PutAsync(id, player);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Never);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is BadRequest response)
        {
            response.Should().NotBeNull().And.BeOfType<BadRequest>();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var id = 1;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(null as PlayerResponseModel);
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PutAsync(id, It.IsAny<PlayerRequestModel>());

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is NotFound response)
        {
            response.Should().NotBeNull().And.BeOfType<NotFound>();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var id = 1;
        var player = PlayerFakes.CreateRequestModelForOneExistingById(id);
        player.FirstName = "Emiliano";
        player.MiddleName = string.Empty;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(PlayerFakes.CreateResponseModelForOneExistingById(id));
        service.Setup(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()));
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PutAsync(id, player);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Once);
        if (result is NoContent response)
        {
            response.Should().NotBeNull().And.BeOfType<NoContent>();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var id = 2;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.DeleteAsync(It.IsAny<long>()), Times.Never);
        if (result is NotFound response)
        {
            response.Should().NotBeNull().And.BeOfType<NotFound>();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var id = 2;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(PlayerFakes.CreateResponseModelForOneExistingById(id));
        service.Setup(service => service.DeleteAsync(id));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<long>()), Times.Once);
        service.Verify(service => service.DeleteAsync(It.IsAny<long>()), Times.Once);
        if (result is NoContent response)
        {
            response.Should().NotBeNull().And.BeOfType<NoContent>();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
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
