using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi
{
    public static class PlayerContextBuilder
    {
        public static PlayerContext CreatePlayerContext(
            DbContextOptions<PlayerContext> dbContextOptions
        )
        {
            return new PlayerContext(dbContextOptions);
        }
    }
}
