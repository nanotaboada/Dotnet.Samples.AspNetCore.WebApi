using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Services
{
    /// <summary>
    /// Interface for managing Player entities in the database context.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Adds a new Player to the database context.
        /// </summary>
        /// <param name="player">The Player to create.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task CreateAsync(Player player);

        /// <summary>
        /// Retrieves all players from the database context.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation,
        /// containing a list of all players.</returns>
        public Task<List<Player>> RetrieveAsync();

        /// <summary>
        /// Retrieves a Player from the database context by its ID.
        /// </summary>
        /// <param name="id">The ID of the Player to retrieve.</param>
        /// <returns>
        /// A ValueTask representing the asynchronous operation, containing the Player if found,
        /// or null if not.
        /// </returns>
        public ValueTask<Player?> RetrieveByIdAsync(long id);

        /// <summary>
        /// Retrieves a Player from the database context by its Squad Number.
        /// </summary>
        /// <param name="squadNumber">The Squad Number of the Player to retrieve.</param>
        /// <returns>
        /// A ValueTask representing the asynchronous operation, containing the Player if found,
        /// or null if not.
        /// </returns>
        public ValueTask<Player?> RetrieveBySquadNumberAsync(int squadNumber);

        /// <summary>
        /// Updates (entirely) an existing Player in the database context.
        /// </summary>
        /// <param name="player">The Player to update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task UpdateAsync(Player player);

        /// <summary>
        /// Removes an existing Player from the database context.
        /// </summary>
        /// <param name="id">The ID of the Player to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task DeleteAsync(long id);
    }
}
