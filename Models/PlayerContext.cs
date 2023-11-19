
using Microsoft.EntityFrameworkCore;

namespace Dotnet.AspNetCore.Samples.WebApi.Models;

public class PlayerContext : DbContext
{
    public PlayerContext(DbContextOptions<PlayerContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; } = null!;
}
