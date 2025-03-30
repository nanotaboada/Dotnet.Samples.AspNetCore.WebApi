using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet.Samples.AspNetCore.WebApi.Services;

public class PlayerService(
    IPlayerRepository playerRepository,
    ILogger<PlayerService> logger,
    IMemoryCache memoryCache
) : IPlayerService
{
    private const string MemoryCache_Key_RetrieveAsync = "MemoryCache_Key_RetrieveAsync";
    private const string AspNetCore_Environment = "ASPNETCORE_ENVIRONMENT";
    private const string Development = "Development";
    private readonly IPlayerRepository _playerRepository = playerRepository;
    private readonly ILogger<PlayerService> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public async Task CreateAsync(Player player)
    {
        await _playerRepository.AddAsync(player);
        _memoryCache.Remove(MemoryCache_Key_RetrieveAsync);
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    public async Task<List<Player>> RetrieveAsync()
    {
        if (_memoryCache.TryGetValue(MemoryCache_Key_RetrieveAsync, out List<Player>? players))
        {
            _logger.Log(LogLevel.Information, "Players retrieved from MemoryCache.");

            return players!;
        }
        else
        {
            /*
                Use multiple environments in ASP.NET Core
                https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-8.0
            */
            if (Environment.GetEnvironmentVariable(AspNetCore_Environment) == Development)
            {
                // Simulates a random delay
                await Task.Delay(new Random().Next(2600, 4200));
            }

            players = await _playerRepository.GetAllAsync();
            _logger.Log(LogLevel.Information, "Players retrieved from DbContext.");

            using (var cacheEntry = _memoryCache.CreateEntry(MemoryCache_Key_RetrieveAsync))
            {
                cacheEntry.Value = players;
                cacheEntry.SetOptions(GetMemoryCacheEntryOptions());
            }

            return players;
        }
    }

    public async ValueTask<Player?> RetrieveByIdAsync(long id) =>
        await _playerRepository.FindByIdAsync(id);

    public async ValueTask<Player?> RetrieveBySquadNumberAsync(int squadNumber) =>
        await _playerRepository.FindBySquadNumberAsync(squadNumber);

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public async Task UpdateAsync(Player player)
    {
        if (await _playerRepository.FindByIdAsync(player.Id) is Player entity)
        {
            entity.MapFrom(player);
            await _playerRepository.UpdateAsync(entity);
            _memoryCache.Remove(MemoryCache_Key_RetrieveAsync);
        }
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    public async Task DeleteAsync(long id)
    {
        if (await _playerRepository.FindByIdAsync(id) is not null)
        {
            await _playerRepository.RemoveAsync(id);
            _memoryCache.Remove(MemoryCache_Key_RetrieveAsync);
        }
    }

    /// <summary>
    /// Creates a MemoryCacheEntryOptions instance with Normal priority,
    /// SlidingExpiration of 10 minutes and AbsoluteExpiration of 1 hour.
    /// </summary>
    /// <returns>A MemoryCacheEntryOptions instance with the specified options.</returns>
    private static MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Normal)
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
    }
}
