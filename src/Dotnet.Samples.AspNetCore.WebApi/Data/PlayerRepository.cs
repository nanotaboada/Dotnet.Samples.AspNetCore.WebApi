using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public sealed class PlayerRepository(PlayerDbContext dbContext)
    : Repository<Player>(dbContext),
        IPlayerRepository
{
    public async Task<Player?> FindBySquadNumberAsync(int squadNumber) =>
        await _dbSet.FirstOrDefaultAsync(p => p.SquadNumber == squadNumber);

    public async Task<bool> SquadNumberExistsAsync(int squadNumber)
    {
        return await dbContext.Players.AnyAsync(p => p.SquadNumber == squadNumber);
    }
}
