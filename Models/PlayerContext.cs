namespace Dotnet.AspNetCore.Samples.WebApi.Models;

using Microsoft.EntityFrameworkCore;

public class PlayerContext : DbContext
{
    public PlayerContext(DbContextOptions<PlayerContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; } = null!;
}
