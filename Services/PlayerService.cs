
using Dotnet.AspNetCore.Samples.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.AspNetCore.Samples.WebApi.Services;

public class PlayerService : IPlayerService
{
    private readonly PlayerContext _playerContext;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(PlayerContext playerContext, ILogger<PlayerService> logger)
    {
        _playerContext = playerContext;
        _logger = logger;
    }

    public async Task Create(Player player)
    {
        _playerContext.Players.Add(player);
        await _playerContext.SaveChangesAsync();
    }

    public Task<List<Player>> Retrieve()
    {
        return _playerContext.Players.ToListAsync();
    }

    public ValueTask<Player?> RetrieveById(long id)
    {
        return _playerContext.Players.FindAsync(id);
    }

    public async Task Update(long id, Player player)
    {
        _playerContext.Entry(player).State = EntityState.Modified;

        try
        {
            await _playerContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            // https://learn.microsoft.com/en-us/ef/core/saving/concurrency
            foreach (var entry in exception.Entries)
            {
                if (entry.Entity is Player)
                {
                    throw new NotImplementedException(
                        "Concurrency conflicts handling not implemented for "
                        + entry.Metadata.Name);
                }
            }
        }
    }

    public async Task Delete(long id)
    {
        var player = await _playerContext.Players.FindAsync(id);

        _playerContext.Players.Remove(player);
        await _playerContext.SaveChangesAsync();
    }

    private bool PlayerExists(long id)
    {
        return (_playerContext.Players?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
