using Dotnet.AspNetCore.Samples.WebApi.Models;
using System.Text.Json;

namespace Dotnet.AspNetCore.Samples.WebApi.Data;

public static class PlayerDataBuilder
{
    public static Player[] SeedWithStarting11()
    {
        var players = new List<Player>();

        var json = """
            [{
                "id": 1,
                "firstName": "Damián",
                "middleName": "Emiliano",
                "lastName": "Martínez",
                "dateOfBirth": "1992-09-01T21:00:00-00:00",
                "squadNumber": 23,
                "position": "Goalkeeper",
                "abbrPosition": "GK",
                "team": "Aston Villa FC",
                "league": "Premier League",
                "starting11": false
            },
            {
                "id": 2,
                "firstName": "Nahuel",
                "middleName": null,
                "lastName": "Molina",
                "dateOfBirth": "1998-04-05T21:00:00-00:00",
                "squadNumber": 26,
                "position": "Right-Back",
                "abbrPosition": "RB",
                "team": "Atlético Madrid",
                "league": "La Liga",
                "starting11": false
            },
            {
                "id": 3,
                "firstName": "Cristian",
                "middleName": "Gabriel",
                "lastName": "Romero",
                "dateOfBirth": "1998-04-26T21:00:00-00:00",
                "squadNumber": 13,
                "position": "Centre-Back",
                "abbrPosition": "CB",
                "team": "Tottenham Hotspur",
                "league": "Premier League",
                "starting11": false
            },
            {
                "id": 4,
                "firstName": "Nicolás",
                "middleName": "Hernán Gonzalo",
                "lastName": "Otamendi",
                "dateOfBirth": "1988-02-11T21:00:00-00:00",
                "squadNumber": 19,
                "position": "Centre-Back",
                "abbrPosition": "CB",
                "team": "SL Benfica",
                "league": "Liga Portugal",
                "starting11": false
            },
            {
                "id": 5,
                "firstName": "Nicolás",
                "middleName": "Alejandro",
                "lastName": "Tagliafico",
                "dateOfBirth": "1992-08-30T21:00:00-00:00",
                "squadNumber": 3,
                "position": "Left-Back",
                "abbrPosition": "LB",
                "team": "Olympique Lyon",
                "league": "Ligue 1",
                "starting11": false
            },
            {
                "id": 6,
                "firstName": "Ángel",
                "middleName": "Fabián",
                "lastName": "Di María",
                "dateOfBirth": "1988-02-13T21:00:00-00:00",
                "squadNumber": 11,
                "position": "Right Winger",
                "abbrPosition": "LW",
                "team": "SL Benfica",
                "league": "Liga Portugal",
                "starting11": false
            },
            {
                "id": 7,
                "firstName": "Rodrigo",
                "middleName": "Javier",
                "lastName": "de Paul",
                "dateOfBirth": "1994-05-23T21:00:00-00:00",
                "squadNumber": 7,
                "position": "Central Midfield",
                "abbrPosition": "CM",
                "team": "Atlético Madrid",
                "league": "La Liga",
                "starting11": false
            },
            {
                "id": 8,
                "firstName": "Enzo",
                "middleName": "Jeremías",
                "lastName": "Fernández",
                "dateOfBirth": "2001-01-16T21:00:00-00:00",
                "squadNumber": 24,
                "position": "Central Midfield",
                "abbrPosition": "CM",
                "team": "Chelsea FC",
                "league": "Premier League",
                "starting11": false
            },
            {
                "id": 9,
                "firstName": "Alexis",
                "middleName": null,
                "lastName": "Mac Allister",
                "dateOfBirth": "1998-12-23T21:00:00-00:00",
                "squadNumber": 20,
                "position": "Central Midfield",
                "abbrPosition": "CM",
                "team": "Liverpool FC",
                "league": "Premier League",
                "starting11": false
            },
            {
                "id": 10,
                "firstName": "Lionel",
                "middleName": "Andrés",
                "lastName": "Messi",
                "dateOfBirth": "1987-06-23T21:00:00-00:00",
                "squadNumber": 10,
                "position": "Right Winger",
                "abbrPosition": "RW",
                "team": "Inter Miami CF",
                "league": "Major League Soccer",
                "starting11": false
            },
            {
                "id": 11,
                "firstName": "Julián",
                "middleName": null,
                "lastName": "Álvarez",
                "dateOfBirth": "2000-01-30T21:00:00-00:00",
                "squadNumber": 9,
                "position": "Centre-Forward",
                "abbrPosition": "CF",
                "team": "Manchester City",
                "league": "Premier League",
                "starting11": false
            }]
        """;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var starting11 = JsonSerializer.Deserialize<List<Player>>(json, options);

        if (starting11 != null && starting11.Any())
        {
            players.AddRange(starting11);
        }

        return players.ToArray();
    }
}
