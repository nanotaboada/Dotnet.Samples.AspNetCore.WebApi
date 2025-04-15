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
        var request = PlayerFakes.MakeRequestModelForCreate();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        validator
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
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
        var result = await controller.PostAsync(request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Never);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is BadRequest httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<BadRequest>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe409Conflict()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var response = PlayerFakes.MakeResponseModelForCreate();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(response);
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
        var result = await controller.PostAsync(request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is Conflict httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<Conflict>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPostAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe201Created()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var response = PlayerFakes.MakeResponseModelForCreate();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(null as PlayerResponseModel);
        service.Setup(service => service.CreateAsync(request)).ReturnsAsync(response);
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
        var result = await controller.PostAsync(request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.CreateAsync(It.IsAny<PlayerRequestModel>()), Times.Once);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is CreatedAtRoute<PlayerRequestModel> httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<Created<PlayerResponseModel>>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            httpResult.Value.Should().BeEquivalentTo(response);
            httpResult.RouteName.Should().Be("GetById");
            httpResult.RouteValues.Should().NotBeNull().And.ContainKey("id");
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
        var response = PlayerFakes.MakeResponseModelsForRetrieve();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync(response);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Once);
        if (result is Ok<List<PlayerResponseModel>> httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<Ok<List<PlayerResponseModel>>>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            httpResult.Value.Should().NotBeNull().And.BeOfType<List<PlayerResponseModel>>();
            httpResult.Value.Should().BeEquivalentTo(response);
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
        if (result is NotFound httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NotFound>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveByIdAsync(id))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetByIdAsync(id);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<Guid>()), Times.Once);
        if (result is NotFound httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NotFound>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetByIdAsync_WhenServiceRetrieveByIdAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = PlayerFakes.MakeResponseModelForRetrieve(10);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveByIdAsync(id)).ReturnsAsync(response);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetByIdAsync(id);

        // Assert
        service.Verify(service => service.RetrieveByIdAsync(It.IsAny<Guid>()), Times.Once);
        if (result is Ok<PlayerResponseModel> httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            httpResult.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
            httpResult.Value.Should().BeEquivalentTo(response);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var squadNumber = 999;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetBySquadNumberAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        if (result is NotFound httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NotFound>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenGetBySquadNumberAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe200Ok()
    {
        // Arrange
        var squadNumber = 10;
        var response = PlayerFakes.MakeResponseModelForRetrieve(squadNumber);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(response);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetBySquadNumberAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        if (result is Ok<PlayerResponseModel> httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<Ok<PlayerResponseModel>>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            httpResult.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
            httpResult.Value.Should().BeEquivalentTo(response);
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
        var squadNumber = 20;
        var request = PlayerFakes.MakeRequestModelForUpdate(squadNumber);
        request.SquadNumber = -999; // Invalid Squad Number
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        validator
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("SquadNumber", "SquadNumber must be greater than 0.")
                    }
                )
            );

        // Act
        var result = await controller.PutAsync(squadNumber, request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Never);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is BadRequest httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<BadRequest>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var squadNumber = 999;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
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
        var result = await controller.PutAsync(squadNumber, It.IsAny<PlayerRequestModel>());

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<PlayerRequestModel>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        if (result is NotFound httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NotFound>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenPutAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var squadNumber = 23;
        var request = PlayerFakes.MakeRequestModelForUpdate(squadNumber);
        var response = PlayerFakes.MakeResponseModelForUpdate(squadNumber);
        request.FirstName = "Emiliano";
        request.MiddleName = string.Empty;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(response);
        service.Setup(service => service.UpdateAsync(request));
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
        var result = await controller.PutAsync(squadNumber, request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Once);
        if (result is NoContent httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NoContent>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsNull_ThenResponseStatusCodeShouldBe404NotFound()
    {
        // Arrange
        var squadNumber = 999;
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(null as PlayerResponseModel);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.DeleteAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.DeleteAsync(It.IsAny<int>()), Times.Never);
        if (result is NotFound httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NotFound>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenDeleteAsync_WhenServiceRetrieveBySquadNumberAsyncReturnsPlayer_ThenResponseStatusCodeShouldBe204NoContent()
    {
        // Arrange
        var squadNumber = 26;
        var response = PlayerFakes.MakeResponseModelForUpdate(squadNumber);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(response);
        service.Setup(service => service.DeleteAsync(squadNumber));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.DeleteAsync(squadNumber);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.DeleteAsync(It.IsAny<int>()), Times.Once);
        if (result is NoContent httpResult)
        {
            httpResult.Should().NotBeNull().And.BeOfType<NoContent>();
            httpResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
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
