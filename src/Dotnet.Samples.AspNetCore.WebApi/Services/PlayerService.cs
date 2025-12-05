using AutoMapper;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Microsoft.Extensions.Caching.Memory;
namespace Dotnet.Samples.AspNetCore.WebApi.Services;

public class PlayerService(
    IPlayerRepository playerRepository,
    ILogger<PlayerService> logger,
    IMemoryCache memoryCache,
    IMapper mapper,
    IHostEnvironment hostEnvironment
) : IPlayerService
{
    /// <summary>
    /// Creates a MemoryCacheEntryOptions instance with Normal priority,
    /// SlidingExpiration of 10 minutes and AbsoluteExpiration of 1 hour.
    /// </summary>
    private static readonly MemoryCacheEntryOptions CacheEntryOptions =
        new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Normal)
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

    /// <summary>
    /// The key used to store the list of Players in the cache.
    /// </summary>
    private static readonly string CacheKey_RetrieveAsync = nameof(RetrieveAsync);

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public async Task<PlayerResponseModel> CreateAsync(PlayerRequestModel playerRequestModel)
    {
        var player = mapper.Map<Player>(playerRequestModel);
        await playerRepository.AddAsync(player);
        logger.LogInformation("Player added to Repository: {Player}", player);
        memoryCache.Remove(CacheKey_RetrieveAsync);
        logger.LogInformation("Removed objects from Cache with Key: {Key}", CacheKey_RetrieveAsync);
        return mapper.Map<PlayerResponseModel>(player);
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    public async Task<List<PlayerResponseModel>> RetrieveAsync()
    {
        if (memoryCache.TryGetValue(CacheKey_RetrieveAsync, out List<PlayerResponseModel>? cached))
        {
            logger.LogInformation("Players retrieved from Cache");
            return cached!;
        }
        else
        {
            if (hostEnvironment.IsDevelopment())
            {
                await SimulateRepositoryDelayAsync();
            }
            var players = await playerRepository.GetAllAsync();
            logger.LogInformation("Players retrieved from Repository");
            var playerResponseModels = mapper.Map<List<PlayerResponseModel>>(players);
            using (var cacheEntry = memoryCache.CreateEntry(CacheKey_RetrieveAsync))
            {
                logger.LogInformation(
                    "{Count} entries created in Cache with key: {Key}",
                    playerResponseModels.Count,
                    CacheKey_RetrieveAsync
                );
                cacheEntry.SetSize(playerResponseModels.Count);
                cacheEntry.Value = playerResponseModels;
                cacheEntry.SetOptions(CacheEntryOptions);
            }
            return playerResponseModels;
        }
    }

    public async Task<PlayerResponseModel?> RetrieveByIdAsync(Guid id)
    {
        var player = await playerRepository.FindByIdAsync(id);
        return player is not null ? mapper.Map<PlayerResponseModel>(player) : null;
    }

    public async Task<PlayerResponseModel?> RetrieveBySquadNumberAsync(int squadNumber)
    {
        var player = await playerRepository.FindBySquadNumberAsync(squadNumber);
        return player is not null ? mapper.Map<PlayerResponseModel>(player) : null;
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public async Task UpdateAsync(PlayerRequestModel playerRequestModel)
    {
        if (
            await playerRepository.FindBySquadNumberAsync(playerRequestModel.SquadNumber)
            is Player player
        )
        {
            mapper.Map(playerRequestModel, player);
            await playerRepository.UpdateAsync(player);
            logger.LogInformation("Player updated in Repository: {Player}", player);
            memoryCache.Remove(CacheKey_RetrieveAsync);
            logger.LogInformation(
                "Removed objects from Cache with Key: {Key}",
                CacheKey_RetrieveAsync
            );
        }
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    public async Task DeleteAsync(int squadNumber)
    {
        if (await playerRepository.FindBySquadNumberAsync(squadNumber) is Player player)
        {
            await playerRepository.RemoveAsync(player.Id);
            logger.LogInformation(
                "Player with Id {SquadNumber} removed from Repository",
                squadNumber
            );
            memoryCache.Remove(CacheKey_RetrieveAsync);
            logger.LogInformation(
                "Removed objects from Cache with Key: {Key}",
                CacheKey_RetrieveAsync
            );
        }
    }

    /// <summary>
    /// Simulates a delay in the repository call to mimic a long-running operation.
    /// This is only used in the Development environment to simulate a delay
    /// in the repository call. In production, this method should not be called.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SimulateRepositoryDelayAsync()
    {
        var milliseconds = new Random().Next(2600, 4200);
        logger.LogInformation(
            "Simulating a random delay of {Milliseconds} milliseconds...",
            milliseconds
        );
        await Task.Delay(milliseconds);
    }
}
