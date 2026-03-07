using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentAssertions;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerValidatorTests
{
    private static PlayerRequestModelValidator CreateValidator(
        Mock<IPlayerRepository>? repositoryMock = null
    )
    {
        var mock = repositoryMock ?? new Mock<IPlayerRepository>();
        return new PlayerRequestModelValidator(mock.Object);
    }

    /* -------------------------------------------------------------------------
     * Valid request
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(null as Player);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /* -------------------------------------------------------------------------
     * FirstName
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_FirstNameEmpty_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.FirstName = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "FirstName");
    }

    /* -------------------------------------------------------------------------
     * LastName
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_LastNameEmpty_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.LastName = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "LastName");
    }

    /* -------------------------------------------------------------------------
     * SquadNumber
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_SquadNumberNotGreaterThanZero_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.SquadNumber = 0;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "SquadNumber");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_SquadNumberNotUnique_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var existingPlayer = PlayerFakes.MakeNew();
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(existingPlayer);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(error =>
                error.PropertyName == "SquadNumber" && error.ErrorMessage.Contains("unique")
            );
    }

    /* -------------------------------------------------------------------------
     * AbbrPosition
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_AbbrPositionEmpty_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.AbbrPosition = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "AbbrPosition");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_AbbrPositionInvalid_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.AbbrPosition = "INVALID";
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "AbbrPosition");
    }

    /* -------------------------------------------------------------------------
     * DateOfBirth
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthNull_ReturnsNoErrors()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = null;
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(null as Player);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthInFuture_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = DateTime.UtcNow.AddYears(1);
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "DateOfBirth");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthToday_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = DateTime.UtcNow.Date; // rule: strictly < UtcNow.Date
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthBeforeYear1900_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "DateOfBirth");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthOnJanuary1st1900_ReturnsNoErrors()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc); // rule: >= 1900-01-01
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(r => r.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(null as Player);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
