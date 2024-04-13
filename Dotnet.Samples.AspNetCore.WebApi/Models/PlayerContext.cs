using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Models;

public class PlayerContext : DbContext
{
    public PlayerContext() { }

    public PlayerContext(DbContextOptions<PlayerContext> options)
        : base(options) { }

    public virtual DbSet<Player> Players { get; set; } = null!;
}
