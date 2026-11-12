using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentAssertions;
using FluentValidation;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerSearchRequestValidatorTests
{
    private readonly PlayerSearchRequestValidator
        _validator = new();

    [Fact]
    [Trait("Category", "Unit")]
    public async Task
        ValidateAsync_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new PlayerSearchRequest
        {
            Team = "Arsenal",
            Position = "CM",
            Page = 1,
            PageSize = 10
        };

        // Act
        var result =
            await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task
        ValidateAsync_PageZero_ReturnsValidationError()
    {
        // Arrange
        var request = new PlayerSearchRequest
        {
            Page = 0,
            PageSize = 10
        };

        // Act
        var result =
            await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(
            error =>
                error.PropertyName
                == nameof(PlayerSearchRequest.Page)
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    [Trait("Category", "Unit")]
    public async Task
        ValidateAsync_PageSizeOutsideRange_ReturnsValidationError(
            int pageSize)
    {
        // Arrange
        var request = new PlayerSearchRequest
        {
            Page = 1,
            PageSize = pageSize
        };

        // Act
        var result =
            await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(
            error =>
                error.PropertyName
                == nameof(PlayerSearchRequest.PageSize)
        );
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task
        ValidateAsync_InvalidPosition_ReturnsValidationError()
    {
        // Arrange
        var request = new PlayerSearchRequest
        {
            Position = "INVALID",
            Page = 1,
            PageSize = 10
        };

        // Act
        var result =
            await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(
            error =>
                error.PropertyName
                == nameof(PlayerSearchRequest.Position)
                && error.ErrorMessage.Contains(
                    "invalid",
                    StringComparison.OrdinalIgnoreCase)
        );
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task
        ValidateAsync_PositionNotProvided_ReturnsNoErrors()
    {
        // Arrange
        var request = new PlayerSearchRequest
        {
            Position = null,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result =
            await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}