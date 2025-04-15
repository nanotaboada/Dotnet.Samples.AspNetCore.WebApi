using System.Text.Json;
using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public static class PlayerData
{
    public static List<Player> MakeStarting11()
    {
        return
        [
            new()
            {
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

    public static List<Player> MakeStarting11WithId()
    {
        return
        [
            new()
            {
                Id = Guid.Parse("f91b9671-cfd1-48d7-afb9-34e93568c9ee"),
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
                Id = Guid.Parse("51ec988a-0d8b-42d9-84e4-5e17c93d8d50"),
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
                Id = Guid.Parse("0969be24-086c-4c51-9c29-0280683b8133"),
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
                Id = Guid.Parse("ac532709-4682-49db-acc2-395f61f405ab"),
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
                Id = Guid.Parse("dc052ee4-c69d-49da-a256-b8ec727f4d59"),
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
                Id = Guid.Parse("6def9bb7-23c2-42b5-b37b-2e9b6fec31cd"),
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
                Id = Guid.Parse("8ca911d9-ab32-4366-b2b1-cad5eb6f4bcc"),
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
                Id = Guid.Parse("198c4774-9607-4e76-8475-ec2528af69d2"),
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
                Id = Guid.Parse("06971ada-1b3d-4d4a-88f5-e2f35311b5aa"),
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
                Id = Guid.Parse("df6f6bab-5efd-4518-80bb-09ef54435636"),
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
                Id = Guid.Parse("27cf4e29-67d5-4c3b-9cf8-7d3fa3942fcb"),
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

    /// <summary>
    /// The purpose of this method is to demonstrate the capabilities of
    /// System.Text.Json.JsonSerializer
    /// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization
    /// </summary>
    /// <returns>A list of Players.</returns>
    public static List<Player> CreateFromDeserializedJson()
    {
        var players = new List<Player>();

        var json = """
                [{
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

        var starting11 = JsonSerializer.Deserialize<List<Player>>(json, options);

        if (starting11 != null && starting11.Count > 0)
        {
            players.AddRange(starting11);
        }

        return players;
    }

    private static readonly JsonSerializerOptions options =
        new() { PropertyNameCaseInsensitive = true };
}
