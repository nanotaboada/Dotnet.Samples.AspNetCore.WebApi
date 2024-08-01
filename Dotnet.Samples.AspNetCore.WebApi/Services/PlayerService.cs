using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet.Samples.AspNetCore.WebApi.Services;

public class PlayerService : IPlayerService
{
    private const string MemoryCache_Key_RetrieveAsync = "MemoryCache_Key_RetrieveAsync";
    private const string EnvironmentVariable_Key = "ASPNETCORE_ENVIRONMENT";
    private const string EnvironmentVariable_Value = "Development";
    private readonly PlayerDbContext _playerContext;
    private readonly ILogger<PlayerService> _logger;
    private readonly IMemoryCache _memoryCache;

    public PlayerService(
        PlayerDbContext playerContext,
        ILogger<PlayerService> logger,
        IMemoryCache memoryCache
    )
    {
        _playerContext = playerContext;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public async Task CreateAsync(Player player)
    {
        _playerContext.Add(player);
        await _playerContext.SaveChangesAsync();
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
            if (
                Environment.GetEnvironmentVariable(EnvironmentVariable_Key)
                == EnvironmentVariable_Value
            )
            {
                // Simulates a random delay
                await Task.Delay(new Random().Next(2600, 4200));
            }

            players = await _playerContext.Players.ToListAsync();
            _logger.Log(LogLevel.Information, "Players retrieved from DbContext.");

            using (var cacheEntry = _memoryCache.CreateEntry(MemoryCache_Key_RetrieveAsync))
            {
                cacheEntry.Value = players;
                cacheEntry.SetOptions(GetMemoryCacheEntryOptions());
            }

            return players;
        }
    }

    public async ValueTask<Player?> RetrieveByIdAsync(long id)
    {
        return await _playerContext.Players.FindAsync(id);
    }

    public async ValueTask<Player?> RetrieveBySquadNumberAsync(int squadNumber)
    {
        return await _playerContext.Players.FirstOrDefaultAsync(player =>
            player.SquadNumber == squadNumber
        );
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public async Task UpdateAsync(Player player)
    {
        if (await _playerContext.Players.FindAsync(player.Id) is Player entity)
        {
            entity.MapFrom(player);
            await _playerContext.SaveChangesAsync();
            _memoryCache.Remove(MemoryCache_Key_RetrieveAsync);
        }
    }

    /* -------------------------------------------------------------------------
     * Delete
     * ---------------------------------------------------------------------- */

    public async Task DeleteAsync(long id)
    {
        if (await _playerContext.Players.FindAsync(id) is Player entity)
        {
            _playerContext.Remove(entity);
            await _playerContext.SaveChangesAsync();
            _memoryCache.Remove(MemoryCache_Key_RetrieveAsync);
        }
    }

    private MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.Normal);
    }
}
