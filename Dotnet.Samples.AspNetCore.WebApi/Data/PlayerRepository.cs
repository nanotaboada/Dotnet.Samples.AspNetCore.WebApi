using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public sealed class PlayerRepository(PlayerDbContext dbContext)
    : Repository<Player>(dbContext),
        IPlayerRepository
{
    public async ValueTask<Player?> FindBySquadNumberAsync(int squadNumber) =>
        await _dbSet.FirstOrDefaultAsync(p => p.SquadNumber == squadNumber);
}
