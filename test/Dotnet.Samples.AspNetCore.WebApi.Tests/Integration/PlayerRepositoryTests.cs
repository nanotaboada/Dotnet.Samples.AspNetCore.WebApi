using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Integration;

/// <summary>
/// Integration tests for <see cref="Repository{T}"/> and <see cref="PlayerRepository"/>.
/// Each test runs against an in-memory SQLite database with the full EF Core migration
/// chain applied via <see cref="DatabaseFakes.MigrateAsync"/>, which also validates
/// that the migration chain itself is healthy as a side effect.
/// </summary>
public class PlayerRepositoryTests : IAsyncLifetime
{
    private DbConnection _connection = default!;
    private PlayerDbContext _dbContext = default!;
    private PlayerRepository _repository = default!;

    public async Task InitializeAsync()
    {
        var (connection, options) = DatabaseFakes.CreateSqliteConnection();
        _connection = connection;
        _dbContext = DatabaseFakes.CreateDbContext(options);
        await _dbContext.MigrateAsync();
        _repository = new PlayerRepository(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }

    /* -------------------------------------------------------------------------
     * GetAllAsync
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAllAsync_WhenCalled_ReturnsAllSeededPlayers()
    {
        // Act
        var players = await _repository.GetAllAsync();

        // Assert
        players.Should().HaveCount(26);
        _dbContext.ChangeTracker.Entries<Player>().Should().BeEmpty();
    }

    /* -------------------------------------------------------------------------
     * FindByIdAsync
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FindByIdAsync_ExistingId_ReturnsPlayer()
    {
        // Arrange — resolve a real ID from the seeded database
        var seeded = await _repository.GetAllAsync();
        var existingId = seeded[0].Id;

        // Act
        var player = await _repository.FindByIdAsync(existingId);

        // Assert
        player.Should().NotBeNull();
        player!.Id.Should().Be(existingId);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FindByIdAsync_UnknownId_ReturnsNull()
    {
        // Act
        var player = await _repository.FindByIdAsync(Guid.NewGuid());

        // Assert
        player.Should().BeNull();
    }

    /* -------------------------------------------------------------------------
     * RemoveAsync
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task RemoveAsync_ExistingEntity_RemovesFromDatabase()
    {
        // Arrange
        var seeded = await _repository.GetAllAsync();
        var existingId = seeded[0].Id;

        // Act
        await _repository.RemoveAsync(existingId);

        // Assert
        var player = await _repository.FindByIdAsync(existingId);
        player.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task RemoveAsync_UnknownId_NoExceptionThrown()
    {
        // Arrange
        var countBefore = (await _repository.GetAllAsync()).Count;

        // Act
        var act = async () => await _repository.RemoveAsync(Guid.NewGuid());

        // Assert
        await act.Should().NotThrowAsync();
        var countAfter = (await _repository.GetAllAsync()).Count;
        countAfter.Should().Be(countBefore);
    }

    /* -------------------------------------------------------------------------
     * FindBySquadNumberAsync
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FindBySquadNumberAsync_ExistingSquadNumber_ReturnsPlayer()
    {
        // Arrange
        var expected = PlayerFakes.MakeFromStarting11(23);

        // Act
        var player = await _repository.FindBySquadNumberAsync(expected.SquadNumber);

        // Assert
        player.Should().NotBeNull();
        player!.SquadNumber.Should().Be(expected.SquadNumber);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FindBySquadNumberAsync_UnknownSquadNumber_ReturnsNull()
    {
        // Arrange — derive a squad number guaranteed not to exist in the seeded data
        var seeded = await _repository.GetAllAsync();
        var unknownSquadNumber = seeded.Max(p => p.SquadNumber) + 1;

        // Act
        var player = await _repository.FindBySquadNumberAsync(unknownSquadNumber);

        // Assert
        player.Should().BeNull();
    }

    /* -------------------------------------------------------------------------
     * SquadNumberExistsAsync
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SquadNumberExistsAsync_ExistingSquadNumber_ReturnsTrue()
    {
        // Arrange
        var expected = PlayerFakes.MakeFromStarting11(23);

        // Act
        var exists = await _repository.SquadNumberExistsAsync(expected.SquadNumber);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SquadNumberExistsAsync_UnknownSquadNumber_ReturnsFalse()
    {
        // Arrange — derive a squad number guaranteed not to exist in the seeded data
        var seeded = await _repository.GetAllAsync();
        var unknownSquadNumber = seeded.Max(p => p.SquadNumber) + 1;

        // Act
        var exists = await _repository.SquadNumberExistsAsync(unknownSquadNumber);

        // Assert
        exists.Should().BeFalse();
    }
}
