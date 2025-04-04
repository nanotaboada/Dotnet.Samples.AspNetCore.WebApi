// IPlayerRepository.cs

using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

/// <summary>
/// Provides specialized repository operations for Player entities.
/// </summary>
public interface IPlayerRepository : IRepository<Player>
{
    /// <summary>
    /// Finds a Player in the repository by their Squad Number.
    /// </summary>
    /// <param name="squadNumber">The Squad Number of the Player to retrieve.</param>
    /// <returns>
    /// A ValueTask representing the asynchronous operation, containing the Player if found,
    /// or null if no Player with the specified Squad Number exists.
    /// </returns>
    ValueTask<Player?> FindBySquadNumberAsync(int squadNumber);
}
