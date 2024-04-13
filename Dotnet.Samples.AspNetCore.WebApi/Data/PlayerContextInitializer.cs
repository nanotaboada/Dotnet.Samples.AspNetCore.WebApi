using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi;

public static class PlayerContextInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var scope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<PlayerContext>();

            if (context != null)
            {
                // https://learn.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
                context.Database.EnsureCreated();

                if (!context.Players.Any())
                {
                    var players = PlayerDataBuilder.SeedWithStarting11();

                    if (players.Any())
                    {
                        context.Players.AddRange(players);
                    }
                }
            }
        }
    }
}
