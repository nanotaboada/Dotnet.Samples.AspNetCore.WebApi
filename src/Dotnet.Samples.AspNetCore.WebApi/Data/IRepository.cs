namespace Dotnet.Samples.AspNetCore.WebApi.Data;

/// <summary>
/// Provides generic repository operations for entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type managed by this repository.</typeparam>
public interface IRepository<T>
    where T : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation,
    /// containing a list of all entities.</returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// Finds an entity on the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// A ValueTask representing the asynchronous operation, containing the entity if found,
    /// or null if no entity with the specified ID exists.
    /// </returns>
    ValueTask<T?> FindByIdAsync(Guid id);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Removes an entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to remove.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task RemoveAsync(Guid id);
}
