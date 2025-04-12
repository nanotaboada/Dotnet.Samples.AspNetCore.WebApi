using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;

/// <summary>
/// A Fake is a working implementation that’s simpler than the real one.
/// It usually has some “real” logic but is not suitable for production
/// (e.g., an in‑memory database instead of a full SQL Server). Fakes are
/// useful when you need behavior that’s closer to reality but still want
/// to avoid external dependencies.
/// </summary>
public static class PlayerFakes
{
    public static List<Player> MakeStarting11()
    {
        return
        [
            new()
            {
                Id = 1,
                FirstName = "Damián",
                MiddleName = "Emiliano",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1992, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 23,
                Position = Position.Goalkeeper.Text,
                AbbrPosition = Position.Goalkeeper.Abbr,
                Team = "Aston Villa FC",
                League = "Premier League",
                Starting11 = true,
            },
            new()
            {
                Id = 2,
                FirstName = "Nahuel",
                LastName = "Molina",
                DateOfBirth = new DateTime(1998, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 26,
                Position = Position.RightBack.Text,
                AbbrPosition = Position.RightBack.Abbr,
                Team = "Altético Madrid",
                League = "La Liga",
                Starting11 = true,
            },
            new()
            {
                Id = 3,
                FirstName = "Cristian",
                MiddleName = "Gabriel",
                LastName = "Romero",
                DateOfBirth = new DateTime(1998, 4, 26, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 13,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "Tottenham Hotspur",
                League = "Premier League",
                Starting11 = true,
            },
            new()
            {
                Id = 4,
                FirstName = "Nicolás",
                MiddleName = "Hernán Gonzalo",
                LastName = "Otamendi",
                DateOfBirth = new DateTime(1988, 2, 11, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 19,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "SL Benfica",
                League = "Liga Portugal",
                Starting11 = true,
            },
            new()
            {
                Id = 5,
                FirstName = "Nicolás",
                MiddleName = "Alejandro",
                LastName = "Tagliafico",
                DateOfBirth = new DateTime(1992, 8, 30, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 3,
                Position = Position.LeftBack.Text,
                AbbrPosition = Position.LeftBack.Abbr,
                Team = "Olympique Lyon",
                League = "Ligue 1",
                Starting11 = true,
            },
            new()
            {
                Id = 6,
                FirstName = "Ángel",
                MiddleName = "Fabián",
                LastName = "Di María",
                DateOfBirth = new DateTime(1988, 2, 13, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 11,
                Position = Position.RightWinger.Text,
                AbbrPosition = Position.RightWinger.Abbr,
                Team = "SL Benfica",
                League = "Liga Portugal",
                Starting11 = true,
            },
            new()
            {
                Id = 7,
                FirstName = "Rodrigo",
                MiddleName = "Javier",
                LastName = "de Paul",
                DateOfBirth = new DateTime(1994, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 7,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Altético Madrid",
                League = "La Liga",
                Starting11 = true,
            },
            new()
            {
                Id = 8,
                FirstName = "Enzo",
                MiddleName = "Jeremías",
                LastName = "Fernández",
                DateOfBirth = new DateTime(2001, 1, 16, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 24,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Chelsea FC",
                League = "Premier League",
                Starting11 = true,
            },
            new()
            {
                Id = 9,
                FirstName = "Alexis",
                LastName = "Mac Allister",
                DateOfBirth = new DateTime(1998, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 20,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Liverpool FC",
                League = "Premier League",
                Starting11 = true,
            },
            new()
            {
                Id = 10,
                FirstName = "Lionel",
                MiddleName = "Andrés",
                LastName = "Messi",
                DateOfBirth = new DateTime(1987, 6, 23, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 10,
                Position = Position.RightWinger.Text,
                AbbrPosition = Position.RightWinger.Abbr,
                Team = "Inter Miami CF",
                League = "Major League Soccer",
                Starting11 = true,
            },
            new()
            {
                Id = 11,
                FirstName = "Julián",
                LastName = "Álvarez",
                DateOfBirth = new DateTime(2000, 1, 30, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 9,
                Position = Position.CentreForward.Text,
                AbbrPosition = Position.CentreForward.Abbr,
                Team = "Manchester City",
                League = "Premier League",
                Starting11 = true,
            }
        ];
    }

    public static Player MakeFromStarting11ById(long id)
    {
        var player =
            MakeStarting11().SingleOrDefault(player => player.Id == id)
            ?? throw new ArgumentNullException($"Player with ID {id} not found.");

        return player;
    }

    public static Player MakeNew()
    {
        return new()
        {
            Id = 12,
            FirstName = "Leandro",
            MiddleName = "Daniel",
            LastName = "Paredes",
            DateOfBirth = new DateTime(1994, 06, 29, 0, 0, 0, DateTimeKind.Utc),
            SquadNumber = 5,
            Position = Position.DefensiveMidfield.Text,
            AbbrPosition = Position.DefensiveMidfield.Abbr,
            Team = "AS Roma",
            League = "Serie A",
            Starting11 = false
        };
    }

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public static PlayerRequestModel MakeRequestModelForCreate()
    {
        var player = MakeNew();

        return new PlayerRequestModel()
        {
            Id = player.Id,
            FirstName = player.FirstName,
            MiddleName = player.MiddleName,
            LastName = player.LastName,
            DateOfBirth = player.DateOfBirth,
            SquadNumber = player.SquadNumber,
            AbbrPosition = player.AbbrPosition,
            Team = player.Team,
            League = player.League
        };
    }

    public static PlayerResponseModel MakeResponseModelForCreate()
    {
        var player = MakeNew();

        return new PlayerResponseModel
        {
            Id = player.Id,
            FullName =
                $"{player.FirstName} {(string.IsNullOrWhiteSpace(player.MiddleName) ? "" : player.MiddleName + " ")}{player.LastName}".Trim(),
            Birth = $"{player.DateOfBirth:MMMM d, yyyy}",
            Dorsal = player.SquadNumber,
            Position = player.Position,
            Club = player.Team,
            League = player.League,
            Starting11 = player.Starting11 ? "Yes" : "No"
        };
    }

    /* -------------------------------------------------------------------------
     * Retrieve
     * ---------------------------------------------------------------------- */

    public static PlayerRequestModel MakeRequestModelForRetrieve(long id)
    {
        var player =
            MakeStarting11().SingleOrDefault(player => player.Id == id)
            ?? throw new ArgumentNullException($"Player with ID {id} not found.");

        return new PlayerRequestModel
        {
            Id = player.Id,
            FirstName = player.FirstName,
            MiddleName = player.MiddleName,
            LastName = player.LastName,
            DateOfBirth = player.DateOfBirth,
            SquadNumber = player.SquadNumber,
            AbbrPosition = player.AbbrPosition,
            Team = player.Team,
            League = player.League
        };
    }

    public static PlayerResponseModel MakeResponseModelForRetrieve(long id)
    {
        var player =
            MakeStarting11().SingleOrDefault(player => player.Id == id)
            ?? throw new ArgumentNullException($"Player with ID {id} not found.");

        return new PlayerResponseModel
        {
            Id = player.Id,
            FullName =
                $"{player.FirstName} {(string.IsNullOrWhiteSpace(player.MiddleName) ? "" : player.MiddleName + " ")}{player.LastName}".Trim(),
            Birth = $"{player.DateOfBirth:MMMM d, yyyy}",
            Dorsal = player.SquadNumber,
            Position = player.Position,
            Club = player.Team,
            League = player.League,
            Starting11 = player.Starting11 ? "Yes" : "No"
        };
    }

    public static List<PlayerResponseModel> MakeResponseModelsForRetrieve() =>
        [
            .. MakeStarting11()
                .Select(player => new PlayerResponseModel
                {
                    Id = player.Id,
                    FullName =
                        $"{player.FirstName} {(string.IsNullOrWhiteSpace(player.MiddleName) ? "" : player.MiddleName + " ")}{player.LastName}".Trim(),
                    Birth = $"{player.DateOfBirth:MMMM d, yyyy}",
                    Dorsal = player.SquadNumber,
                    Position = player.Position,
                    Club = player.Team,
                    League = player.League,
                    Starting11 = player.Starting11 ? "Yes" : "No"
                })
        ];

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public static PlayerRequestModel MakeRequestModelForUpdate(long id)
    {
        return MakeRequestModelForRetrieve(id);
    }

    public static PlayerResponseModel MakeResponseModelForUpdate(long id)
    {
        return MakeResponseModelForRetrieve(id);
    }
}
