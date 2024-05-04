using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public class PlayerDbContext : DbContext
{
    public PlayerDbContext(DbContextOptions<PlayerDbContext> options)
        : base(options) { }

    public DbSet<Player> Players => Set<Player>();
}
