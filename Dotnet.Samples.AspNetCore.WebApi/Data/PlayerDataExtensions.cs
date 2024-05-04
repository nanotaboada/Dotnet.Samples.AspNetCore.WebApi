using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public static class PlayerDataExtensions
{
    /// <summary>
    /// Simple extension method to map all properties of a Player
    /// </summary>
    public static void MapFrom(this Player target, Player source)
    {
        target.FirstName = source.FirstName;
        target.MiddleName = source.MiddleName;
        target.LastName = source.LastName;
        target.DateOfBirth = source.DateOfBirth;
        target.SquadNumber = source.SquadNumber;
        target.Position = source.Position;
        target.AbbrPosition = source.AbbrPosition;
        target.Team = source.Team;
        target.League = source.League;
        target.Starting11 = source.Starting11;
    }
}
