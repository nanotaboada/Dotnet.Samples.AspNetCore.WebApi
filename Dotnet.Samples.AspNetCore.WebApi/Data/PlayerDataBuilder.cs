using System.Text.Json;
using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public static class PlayerDataBuilder
{
    public static Player SeedOneById(int id)
    {
        return SeedWithStarting11().SingleOrDefault(player => player.Id == id) ?? new Player();
    }

    public static Player SeedOneNew() =>
        new()
        {
            Id = 12,
            FirstName = "Leandro",
            MiddleName = "Daniel",
            LastName = "Paredes",
            DateOfBirth = new DateTime(1994, 06, 29, 0, 0, 0, DateTimeKind.Utc),
            SquadNumber = 5,
            Position = "Defensive Midfield",
            AbbrPosition = "DM",
            Team = "AS Roma",
            League = "Serie A",
            Starting11 = false
        };

    public static List<Player> SeedWithStarting11()
    {
        var players = new List<Player>
        {
            new()
            {
                Id = 1,
                FirstName = "Damián",
                MiddleName = "Emiliano",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1992, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 23,
                Position = "Goalkeeper",
                AbbrPosition = "GK",
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
                Position = "Right-Back",
                AbbrPosition = "RB",
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
                Position = "Centre-Back",
                AbbrPosition = "CB",
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
                Position = "Centre-Back",
                AbbrPosition = "CB",
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
                Position = "Left-Back",
                AbbrPosition = "LB",
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
                Position = "Right Winger",
                AbbrPosition = "RW",
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
                Position = "Central Midfield",
                AbbrPosition = "CM",
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
                Position = "Central Midfield",
                AbbrPosition = "CM",
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
                Position = "Central Midfield",
                AbbrPosition = "CM",
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
                Position = "Right Winger",
                AbbrPosition = "RW",
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
                Position = "Centre-Forward",
                AbbrPosition = "CF",
                Team = "Manchester City",
                League = "Premier League",
                Starting11 = true,
            }
        };

        return players;
    }

    public static List<Player> SeedWithDeserializedJson()
    {
        var players = new List<Player>();

        var json = """
                [{
                    "id": 1,
                    "firstName": "Damián",
                    "middleName": "Emiliano",
                    "lastName": "Martínez",
                    "dateOfBirth": "1992-09-01T00:00:00.000Z",
                    "squadNumber": 23,
                    "position": "Goalkeeper",
                    "abbrPosition": "GK",
                    "team": "Aston Villa FC",
                    "league": "Premier League",
                    "starting11": true
                },
                {
                    "id": 2,
                    "firstName": "Nahuel",
                    "middleName": null,
                    "lastName": "Molina",
                    "dateOfBirth": "1998-04-05T00:00:00.000Z",
                    "squadNumber": 26,
                    "position": "Right-Back",
                    "abbrPosition": "RB",
                    "team": "Atlético Madrid",
                    "league": "La Liga",
                    "starting11": true
                },
                {
                    "id": 3,
                    "firstName": "Cristian",
                    "middleName": "Gabriel",
                    "lastName": "Romero",
                    "dateOfBirth": "1998-04-26T00:00:00.000Z",
                    "squadNumber": 13,
                    "position": "Centre-Back",
                    "abbrPosition": "CB",
                    "team": "Tottenham Hotspur",
                    "league": "Premier League",
                    "starting11": true
                },
                {
                    "id": 4,
                    "firstName": "Nicolás",
                    "middleName": "Hernán Gonzalo",
                    "lastName": "Otamendi",
                    "dateOfBirth": "1988-02-11T00:00:00.000Z",
                    "squadNumber": 19,
                    "position": "Centre-Back",
                    "abbrPosition": "CB",
                    "team": "SL Benfica",
                    "league": "Liga Portugal",
                    "starting11": true
                },
                {
                    "id": 5,
                    "firstName": "Nicolás",
                    "middleName": "Alejandro",
                    "lastName": "Tagliafico",
                    "dateOfBirth": "1992-08-30T00:00:00.000Z",
                    "squadNumber": 3,
                    "position": "Left-Back",
                    "abbrPosition": "LB",
                    "team": "Olympique Lyon",
                    "league": "Ligue 1",
                    "starting11": true
                },
                {
                    "id": 6,
                    "firstName": "Ángel",
                    "middleName": "Fabián",
                    "lastName": "Di María",
                    "dateOfBirth": "1988-02-13T00:00:00.000Z",
                    "squadNumber": 11,
                    "position": "Right Winger",
                    "abbrPosition": "RW",
                    "team": "SL Benfica",
                    "league": "Liga Portugal",
                    "starting11": true
                },
                {
                    "id": 7,
                    "firstName": "Rodrigo",
                    "middleName": "Javier",
                    "lastName": "de Paul",
                    "dateOfBirth": "1994-05-23T00:00:00.000Z",
                    "squadNumber": 7,
                    "position": "Central Midfield",
                    "abbrPosition": "CM",
                    "team": "Atlético Madrid",
                    "league": "La Liga",
                    "starting11": true
                },
                {
                    "id": 8,
                    "firstName": "Enzo",
                    "middleName": "Jeremías",
                    "lastName": "Fernández",
                    "dateOfBirth": "2001-01-16T00:00:00.000Z",
                    "squadNumber": 24,
                    "position": "Central Midfield",
                    "abbrPosition": "CM",
                    "team": "Chelsea FC",
                    "league": "Premier League",
                    "starting11": true
                },
                {
                    "id": 9,
                    "firstName": "Alexis",
                    "middleName": null,
                    "lastName": "Mac Allister",
                    "dateOfBirth": "1998-12-23T00:00:00.000Z",
                    "squadNumber": 20,
                    "position": "Central Midfield",
                    "abbrPosition": "CM",
                    "team": "Liverpool FC",
                    "league": "Premier League",
                    "starting11": true
                },
                {
                    "id": 10,
                    "firstName": "Lionel",
                    "middleName": "Andrés",
                    "lastName": "Messi",
                    "dateOfBirth": "1987-06-23T00:00:00.000Z",
                    "squadNumber": 10,
                    "position": "Right Winger",
                    "abbrPosition": "RW",
                    "team": "Inter Miami CF",
                    "league": "Major League Soccer",
                    "starting11": true
                },
                {
                    "id": 11,
                    "firstName": "Julián",
                    "middleName": null,
                    "lastName": "Álvarez",
                    "dateOfBirth": "2000-01-30T00:00:00.000Z",
                    "squadNumber": 9,
                    "position": "Centre-Forward",
                    "abbrPosition": "CF",
                    "team": "Manchester City",
                    "league": "Premier League",
                    "starting11": true
                }]
            """;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var starting11 = JsonSerializer.Deserialize<List<Player>>(json, options);

        if (starting11 != null && starting11.Any())
        {
            players.AddRange(starting11);
        }

        return players;
    }
}
