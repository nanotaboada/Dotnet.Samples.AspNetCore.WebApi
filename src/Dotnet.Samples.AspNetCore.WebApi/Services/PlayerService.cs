using AutoMapper;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet.Samples.AspNetCore.WebApi.Services;

public class PlayerService(
    IPlayerRepository playerRepository,
    ILogger<PlayerService> logger,
    IMemoryCache memoryCache,
    IMapper mapper
) : IPlayerService
{
    private static readonly string CacheKey_RetrieveAsync = nameof(RetrieveAsync);
    private static readonly string AspNetCore_Environment = "ASPNETCORE_ENVIRONMENT";
    private static readonly string Development = "Development";

    private readonly IPlayerRepository _playerRepository = playerRepository;
    private readonly ILogger<PlayerService> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IMapper _mapper = mapper;

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public async Task<PlayerResponseModel> CreateAsync(PlayerRequestModel playerRequestModel)
    {
        var player = _mapper.Map<Player>(playerRequestModel);
        await _playerRepository.AddAsync(player);
        _logger.LogInformation("Player added to Repository: {Player}", player);
        _memoryCache.Remove(CacheKey_RetrieveAsync);
        _logger.LogInformation(
            "Removed objects from Cache with Key: {Key}",
            CacheKey_RetrieveAsync
        );
        return _mapper.Map<PlayerResponseModel>(player);
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    public async Task<List<PlayerResponseModel>> RetrieveAsync()
    {
        if (_memoryCache.TryGetValue(CacheKey_RetrieveAsync, out List<PlayerResponseModel>? cached))
        {
            _logger.LogInformation("Players retrieved from Cache");
            return cached!;
        }
        else
        {
            // Use multiple environments in ASP.NET Core
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-8.0
            if (Environment.GetEnvironmentVariable(AspNetCore_Environment) == Development)
            {
                await SimulateRepositoryDelayAsync();
            }

            var players = await _playerRepository.GetAllAsync();
            _logger.LogInformation("Players retrieved from Repository");
            var playerResponseModels = _mapper.Map<List<PlayerResponseModel>>(players);
            using (var cacheEntry = _memoryCache.CreateEntry(CacheKey_RetrieveAsync))
            {
                _logger.LogInformation(
                    "{Count} entries created in Cache with key: {Key}",
                    playerResponseModels.Count,
                    CacheKey_RetrieveAsync
                );
                cacheEntry.SetSize(playerResponseModels.Count);
                cacheEntry.Value = playerResponseModels;
                cacheEntry.SetOptions(GetMemoryCacheEntryOptions());
            }
            return playerResponseModels;
        }
    }

    public async Task<PlayerResponseModel?> RetrieveByIdAsync(long id)
    {
        var player = await _playerRepository.FindByIdAsync(id);
        return player is not null ? _mapper.Map<PlayerResponseModel>(player) : null;
    }

    public async Task<PlayerResponseModel?> RetrieveBySquadNumberAsync(int squadNumber)
    {
        var player = await _playerRepository.FindBySquadNumberAsync(squadNumber);
        return player is not null ? _mapper.Map<PlayerResponseModel>(player) : null;
    }

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public async Task UpdateAsync(PlayerRequestModel playerRequestModel)
    {
        if (await _playerRepository.FindByIdAsync(playerRequestModel.Id) is Player player)
        {
            _mapper.Map(playerRequestModel, player);
            await _playerRepository.UpdateAsync(player);
            _logger.LogInformation("Player updated in Repository: {Player}", player);
            _memoryCache.Remove(CacheKey_RetrieveAsync);
            _logger.LogInformation(
                "Removed objects from Cache with Key: {Key}",
                CacheKey_RetrieveAsync
            );
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
            _logger.LogInformation("Player with Id {Id} removed from Repository", id);
            _memoryCache.Remove(CacheKey_RetrieveAsync);
            _logger.LogInformation(
                "Removed objects from Cache with Key: {Key}",
                CacheKey_RetrieveAsync
            );
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

    /// <summary>
    /// Simulates a delay in the repository call to mimic a long-running operation.
    /// This is only used in the Development environment to simulate a delay
    /// in the repository call. In production, this method should not be called.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SimulateRepositoryDelayAsync()
    {
        var milliseconds = new Random().Next(2600, 4200);
        _logger.LogInformation(
            "Simulating a random delay of {Milliseconds} milliseconds...",
            milliseconds
        );
        await Task.Delay(milliseconds);
    }
}
