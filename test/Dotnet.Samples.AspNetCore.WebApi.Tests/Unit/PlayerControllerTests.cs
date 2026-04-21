using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public async Task Post_Players_ValidationError_Returns422UnprocessableEntity()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("SquadNumber", "SquadNumber must be greater than 0."),
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
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
        var problemDetails = httpResult
            .ProblemDetails.Should()
            .BeOfType<HttpValidationProblemDetails>()
            .Subject;
        problemDetails.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        problemDetails
            .Errors.Should()
            .ContainKey("SquadNumber")
            .WhoseValue.Should()
            .Contain("SquadNumber must be greater than 0.");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Post_Players_Existing_Returns409Conflict()
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
                    It.IsAny<IValidationContext>(),
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
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        var httpResult = result.Should().BeOfType<Conflict<ProblemDetails>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Post_Players_Nonexistent_Returns201Created()
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
                    It.IsAny<IValidationContext>(),
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
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        var httpResult = result.Should().BeOfType<CreatedAtRoute<PlayerResponseModel>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        httpResult.Value.Should().BeEquivalentTo(response);
        httpResult.RouteName.Should().Be("RetrieveBySquadNumber");
        httpResult.RouteValues.Should().NotBeNull().And.ContainKey("squadNumber");
        httpResult.RouteValues!["squadNumber"].Should().Be(response.Dorsal);
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_Players_Existing_ReturnsPlayers()
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
        var httpResult = result.Should().BeOfType<Ok<List<PlayerResponseModel>>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        httpResult.Value.Should().NotBeNull().And.BeOfType<List<PlayerResponseModel>>();
        httpResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_Players_Empty_Returns200OkWithEmptyList()
    {
        // Arrange
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service.Setup(service => service.RetrieveAsync()).ReturnsAsync([]);

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Once);
        var httpResult = result.Should().BeOfType<Ok<List<PlayerResponseModel>>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        httpResult.Value.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_PlayerById_Unknown_Returns404NotFound()
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
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_PlayerById_Existing_Returns200OK()
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
        var httpResult = result.Should().BeOfType<Ok<PlayerResponseModel>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        httpResult.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
        httpResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_PlayerBySquadNumber_Unknown_Returns404NotFound()
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
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Get_PlayerBySquadNumber_Existing_Returns200OK()
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
        var httpResult = result.Should().BeOfType<Ok<PlayerResponseModel>>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        httpResult.Value.Should().NotBeNull().And.BeOfType<PlayerResponseModel>();
        httpResult.Value.Should().BeEquivalentTo(response);
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Put_PlayerBySquadNumber_ValidationError_Returns422UnprocessableEntity()
    {
        // Arrange
        var squadNumber = 20;
        var request = PlayerFakes.MakeRequestModelForUpdate(squadNumber);
        request.SquadNumber = -999; // Invalid Squad Number
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("SquadNumber", "SquadNumber must be greater than 0."),
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
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
        var problemDetails = httpResult
            .ProblemDetails.Should()
            .BeOfType<HttpValidationProblemDetails>()
            .Subject;
        problemDetails.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        problemDetails
            .Errors.Should()
            .ContainKey("SquadNumber")
            .WhoseValue.Should()
            .Contain("SquadNumber must be greater than 0.");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Put_PlayerBySquadNumber_Unknown_Returns404NotFound()
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
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PutAsync(
            squadNumber,
            new PlayerRequestModel { SquadNumber = squadNumber }
        );

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Once);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        validator.Verify(
            validator =>
                validator.ValidateAsync(
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Put_PlayerBySquadNumber_SquadNumberMismatch_Returns400BadRequest()
    {
        // Arrange
        var squadNumber = 23;
        var request = PlayerFakes.MakeRequestModelForUpdate(squadNumber);
        request.SquadNumber = 99; // Mismatched squad number
        var response = PlayerFakes.MakeResponseModelForUpdate(squadNumber);
        var (service, logger, validator) = PlayerMocks.InitControllerMocks();
        service
            .Setup(service => service.RetrieveBySquadNumberAsync(squadNumber))
            .ReturnsAsync(response);
        validator
            .Setup(validator =>
                validator.ValidateAsync(
                    It.IsAny<IValidationContext>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { }));

        var controller = new PlayerController(service.Object, logger.Object, validator.Object);

        // Act
        var result = await controller.PutAsync(squadNumber, request);

        // Assert
        service.Verify(service => service.RetrieveBySquadNumberAsync(It.IsAny<int>()), Times.Never);
        service.Verify(service => service.UpdateAsync(It.IsAny<PlayerRequestModel>()), Times.Never);
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Put_PlayerBySquadNumber_Existing_Returns204NoContent()
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
                    It.IsAny<IValidationContext>(),
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
        var httpResult = result.Should().BeOfType<NoContent>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Delete_PlayerBySquadNumber_Unknown_Returns404NotFound()
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
        var httpResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Delete_PlayerBySquadNumber_Existing_Returns204NoContent()
    {
        // Arrange
        var response = PlayerFakes.MakeResponseModelForCreate();
        var squadNumber = response.Dorsal;
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
        var httpResult = result.Should().BeOfType<NoContent>().Subject;
        httpResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
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
