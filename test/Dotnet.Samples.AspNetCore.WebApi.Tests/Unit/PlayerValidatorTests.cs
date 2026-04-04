using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Unit;

public class PlayerValidatorTests
{
    private sealed class FakeTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    private static PlayerRequestModelValidator CreateValidator(
        Mock<IPlayerRepository>? repositoryMock = null,
        TimeProvider? timeProvider = null
    )
    {
        var mock = repositoryMock ?? new Mock<IPlayerRepository>();
        return new PlayerRequestModelValidator(mock.Object, timeProvider);
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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "SquadNumber");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_SquadNumberNegative_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.SquadNumber = -5;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

        // Assert
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(error =>
                error.PropertyName == "SquadNumber" && error.ErrorMessage.Contains("greater than 0")
            );
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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

        // Assert
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(error =>
                error.PropertyName == "SquadNumber" && error.ErrorMessage.Contains("unique")
            );
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_SquadNumberBelongsToPlayerBeingUpdated_ReturnsNoErrors()
    {
        // Arrange
        // Simulate a PUT request for an existing player: the squad number in the
        // body matches the one already in the database. The "Update" rule set must
        // not run BeUniqueSquadNumber, otherwise this valid request would be rejected.
        var request = PlayerFakes.MakeRequestModelForUpdate(10);
        var existingPlayer = PlayerFakes.MakeFromStarting11(10);
        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.FindBySquadNumberAsync(request.SquadNumber))
            .ReturnsAsync(existingPlayer);
        var validator = CreateValidator(repositoryMock);

        // Act
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Update")
        );

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_FirstNameEmptyInUpdateRuleSet_ReturnsValidationError()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForUpdate(10);
        request.FirstName = string.Empty;
        var validator = CreateValidator();

        // Act
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Update")
        );

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "FirstName");
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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "DateOfBirth");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ValidateAsync_DateOfBirthToday_ReturnsValidationError()
    {
        // Arrange
        var fixedNow = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(fixedNow);
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.DateOfBirth = fixedNow.Date; // same "today" the validator sees
        var validator = CreateValidator(timeProvider: timeProvider);

        // Act
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

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
        var result = await validator.ValidateAsync(
            request,
            options => options.IncludeRuleSets("Create")
        );

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
