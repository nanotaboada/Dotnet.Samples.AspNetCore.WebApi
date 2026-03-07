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
    public async Task GivenValidateAsync_WhenRequestModelIsValid_ThenValidationShouldPass()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
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

    /* -------------------------------------------------------------------------
     * FirstName
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenFirstNameIsEmpty_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.FirstName = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
    }

    /* -------------------------------------------------------------------------
     * LastName
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenLastNameIsEmpty_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.LastName = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName");
    }

    /* -------------------------------------------------------------------------
     * SquadNumber
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenSquadNumberIsNotGreaterThanZero_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.SquadNumber = 0;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SquadNumber");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenSquadNumberIsNotUnique_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        var existingPlayer = PlayerFakes.MakeNew();
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(r => r.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(existingPlayer);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(e => e.PropertyName == "SquadNumber" && e.ErrorMessage.Contains("unique"));
    }

    /* -------------------------------------------------------------------------
     * AbbrPosition
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenAbbrPositionIsEmpty_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.AbbrPosition = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AbbrPosition");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenAbbrPositionIsInvalid_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.AbbrPosition = "INVALID";
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AbbrPosition");
    }

    /* -------------------------------------------------------------------------
     * DateOfBirth
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenDateOfBirthIsNull_ThenValidationShouldPass()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = null;
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(r => r.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(null as Player);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenDateOfBirthIsInTheFuture_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = DateTime.UtcNow.AddYears(1);
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GivenValidateAsync_WhenDateOfBirthIsBeforeYear1900_ThenValidationShouldFail()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = new DateTime(1899, 12, 31);
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
    }
}
