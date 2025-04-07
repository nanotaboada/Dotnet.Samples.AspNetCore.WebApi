using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Services
{
    /// <summary>
    /// Interface for managing Player entities in the repository.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Adds a new Player to the repository.
        /// </summary>
        /// <param name="playerRequestModel">The Player to create.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task CreateAsync(PlayerRequestModel playerRequestModel);

        /// <summary>
        /// Retrieves all players from the repository.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation,
        /// containing a list of all players.</returns>
        public Task<List<PlayerResponseModel>> RetrieveAsync();

        /// <summary>
        /// Retrieves a Player from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the Player to retrieve.</param>
        /// <returns>
        /// A Task representing the asynchronous operation, containing the Player if found,
        /// or null if not.
        /// </returns>
        public Task<PlayerResponseModel?> RetrieveByIdAsync(long id);

        /// <summary>
        /// Retrieves a Player from the repository by its Squad Number.
        /// </summary>
        /// <param name="squadNumber">The Squad Number of the Player to retrieve.</param>
        /// <returns>
        /// A Task representing the asynchronous operation, containing the Player if found,
        /// or null if not.
        /// </returns>
        public Task<PlayerResponseModel?> RetrieveBySquadNumberAsync(int squadNumber);

        /// <summary>
        /// Updates (entirely) an existing Player in the repository.
        /// </summary>
        /// <param name="playerRequestModel">The Player to update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task UpdateAsync(PlayerRequestModel playerRequestModel);

        /// <summary>
        /// Removes an existing Player from the repository.
        /// </summary>
        /// <param name="id">The ID of the Player to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task DeleteAsync(long id);
    }
}
