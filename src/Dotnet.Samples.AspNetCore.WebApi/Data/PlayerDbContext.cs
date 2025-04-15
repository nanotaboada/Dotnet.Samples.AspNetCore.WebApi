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

    /// <summary>
    /// Configures the model for the Player entity.
    /// This method is called by the runtime to configure the model for the context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <remarks>
    /// This method is used to configure the model and relationships using the Fluent API.
    /// It is called when the model for a derived context is being created.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(player => player.Id);
            entity.Property(player => player.Id).ValueGeneratedOnAdd();
            entity.HasIndex(player => player.SquadNumber).IsUnique();
        });
    }
}
