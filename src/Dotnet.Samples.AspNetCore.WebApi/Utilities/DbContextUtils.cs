using Dotnet.Samples.AspNetCore.WebApi.Data;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities;

public static class DbContextUtils
{
    public static void Seed(PlayerDbContext context)
    {
        if (!context.Players.Any())
        {
            context.Players.AddRange(PlayerData.MakeStarting11());
            context.SaveChanges();
        }
    }
}
