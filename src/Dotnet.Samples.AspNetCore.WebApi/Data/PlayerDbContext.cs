using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

/// <summary>
/// Represents the EF Core database context for a Player entity.
/// Inherits from <see cref="DbContext"/> and provides a bridge between the entity and the database.
/// </summary>
public class PlayerDbContext(DbContextOptions<PlayerDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing a collection of Player entities.
    /// <see cref="DbSet{TEntity}"/> corresponds to a table in the database, allowing CRUD operations and LINQ queries.
    /// </summary>
    public DbSet<Player> Players => Set<Player>();
}
