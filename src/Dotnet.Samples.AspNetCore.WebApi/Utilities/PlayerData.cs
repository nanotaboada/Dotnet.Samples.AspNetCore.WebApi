using System.Text.Json;
using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Utilities;

/// <summary>
/// Provides static player data for database seeding and testing.
/// Single source of truth for all player definitions.
/// </summary>
public static class PlayerData
{
    /// <summary>
    /// Returns the starting 11 players without IDs (for EF Core auto-increment).
    /// Used for database migrations and seeding.
    /// </summary>
    /// <returns>List of 11 Player entities representing the starting lineup.</returns>
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
                Team = "SL Benfica",
                League = "Liga Portugal",
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
                Team = "Brighton & Hove Albion",
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
                Team = "Paris Saint-Germain",
                League = "Ligue 1",
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
            },
        ];
    }

    /// <summary>
    /// Create a predefined starting eleven of players where each player has a fixed GUID identifier and full profile data.
    /// </summary>
    /// <returns>A list of 11 Player instances representing the starting lineup; each entry includes a predefined Id (Guid) and populated fields such as name, date of birth, squad number, position (and abbreviation), team, league, and Starting11 set to true.</returns>
    public static List<Player> MakeStarting11WithId()
    {
        return
        [
            new()
            {
                Id = Guid.Parse("01772c59-43f0-5d85-b913-c78e4e281452"),
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
                Id = Guid.Parse("da31293b-4c7e-5e0f-a168-469ee29ecbc4"),
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
                Id = Guid.Parse("c096c69e-762b-5281-9290-bb9c167a24a0"),
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
                Id = Guid.Parse("d5f7dd7a-1dcb-5960-ba27-e34865b63358"),
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
                Id = Guid.Parse("2f6f90a0-9b9d-5023-96d2-a2aaf03143a6"),
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
                Id = Guid.Parse("b5b46e79-929e-5ed2-949d-0d167109c022"),
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
                Id = Guid.Parse("0293b282-1da8-562e-998e-83849b417a42"),
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
                Id = Guid.Parse("d3ba552a-dac3-588a-b961-1ea7224017fd"),
                FirstName = "Enzo",
                MiddleName = "Jeremías",
                LastName = "Fernández",
                DateOfBirth = new DateTime(2001, 1, 16, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 24,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "SL Benfica",
                League = "Liga Portugal",
                Starting11 = true,
            },
            new()
            {
                Id = Guid.Parse("9613cae9-16ab-5b54-937e-3135123b9e0d"),
                FirstName = "Alexis",
                LastName = "Mac Allister",
                DateOfBirth = new DateTime(1998, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 20,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Brighton & Hove Albion",
                League = "Premier League",
                Starting11 = true,
            },
            new()
            {
                Id = Guid.Parse("acc433bf-d505-51fe-831e-45eb44c4d43c"),
                FirstName = "Lionel",
                MiddleName = "Andrés",
                LastName = "Messi",
                DateOfBirth = new DateTime(1987, 6, 23, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 10,
                Position = Position.RightWinger.Text,
                AbbrPosition = Position.RightWinger.Abbr,
                Team = "Paris Saint-Germain",
                League = "Ligue 1",
                Starting11 = true,
            },
            new()
            {
                Id = Guid.Parse("38bae91d-8519-55a2-b30a-b9fe38849bfb"),
                FirstName = "Julián",
                LastName = "Álvarez",
                DateOfBirth = new DateTime(2000, 1, 30, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 9,
                Position = Position.CentreForward.Text,
                AbbrPosition = Position.CentreForward.Abbr,
                Team = "Manchester City",
                League = "Premier League",
                Starting11 = true,
            },
        ];
    }

    /// <summary>
    /// Create a predefined list of 15 substitute players with full profile data but without Id values.
    /// </summary>
    /// <returns>A list of 15 Player instances representing substitute players; each entry includes populated fields such as name, date of birth, squad number, position (and abbreviation), team, league, and Starting11 set to false. Id values are not assigned.</returns>
    public static List<Player> GetSubstitutes()
    {
        return
        [
            new()
            {
                FirstName = "Franco",
                MiddleName = "Daniel",
                LastName = "Armani",
                DateOfBirth = new DateTime(1986, 10, 16, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 1,
                Position = Position.Goalkeeper.Text,
                AbbrPosition = Position.Goalkeeper.Abbr,
                Team = "River Plate",
                League = "Copa de la Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Gerónimo",
                LastName = "Rulli",
                DateOfBirth = new DateTime(1992, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 12,
                Position = Position.Goalkeeper.Text,
                AbbrPosition = Position.Goalkeeper.Abbr,
                Team = "Ajax Amsterdam",
                League = "Eredivisie",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Juan",
                MiddleName = "Marcos",
                LastName = "Foyth",
                DateOfBirth = new DateTime(1998, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 2,
                Position = Position.RightBack.Text,
                AbbrPosition = Position.RightBack.Abbr,
                Team = "Villarreal",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Gonzalo",
                MiddleName = "Ariel",
                LastName = "Montiel",
                DateOfBirth = new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 4,
                Position = Position.RightBack.Text,
                AbbrPosition = Position.RightBack.Abbr,
                Team = "Nottingham Forest",
                League = "Premier League",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Germán",
                MiddleName = "Alejo",
                LastName = "Pezzella",
                DateOfBirth = new DateTime(1991, 6, 27, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 6,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "Real Betis Balompié",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Marcos",
                MiddleName = "Javier",
                LastName = "Acuña",
                DateOfBirth = new DateTime(1991, 10, 28, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 8,
                Position = Position.LeftBack.Text,
                AbbrPosition = Position.LeftBack.Abbr,
                Team = "Sevilla FC",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Lisandro",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1998, 1, 18, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 25,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "Manchester United",
                League = "Premier League",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Leandro",
                MiddleName = "Daniel",
                LastName = "Paredes",
                DateOfBirth = new DateTime(1994, 6, 29, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 5,
                Position = Position.DefensiveMidfield.Text,
                AbbrPosition = Position.DefensiveMidfield.Abbr,
                Team = "AS Roma",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Exequiel",
                MiddleName = "Alejandro",
                LastName = "Palacios",
                DateOfBirth = new DateTime(1998, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 14,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Bayer 04 Leverkusen",
                League = "Bundesliga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Alejandro",
                MiddleName = "Darío",
                LastName = "Gómez",
                DateOfBirth = new DateTime(1988, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 17,
                Position = Position.LeftWinger.Text,
                AbbrPosition = Position.LeftWinger.Abbr,
                Team = "AC Monza",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Guido",
                LastName = "Rodríguez",
                DateOfBirth = new DateTime(1994, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 18,
                Position = Position.DefensiveMidfield.Text,
                AbbrPosition = Position.DefensiveMidfield.Abbr,
                Team = "Real Betis Balompié",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Ángel",
                MiddleName = "Martín",
                LastName = "Correa",
                DateOfBirth = new DateTime(1995, 3, 9, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 15,
                Position = Position.RightWinger.Text,
                AbbrPosition = Position.RightWinger.Abbr,
                Team = "Atlético Madrid",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Thiago",
                MiddleName = "Ezequiel",
                LastName = "Almada",
                DateOfBirth = new DateTime(2001, 4, 26, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 16,
                Position = Position.AttackingMidfield.Text,
                AbbrPosition = Position.AttackingMidfield.Abbr,
                Team = "Atlanta United FC",
                League = "Major League Soccer",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Paulo",
                MiddleName = "Exequiel",
                LastName = "Dybala",
                DateOfBirth = new DateTime(1993, 11, 15, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 21,
                Position = Position.SecondStriker.Text,
                AbbrPosition = Position.SecondStriker.Abbr,
                Team = "AS Roma",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Lautaro",
                MiddleName = "Javier",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1997, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 22,
                Position = Position.CentreForward.Text,
                AbbrPosition = Position.CentreForward.Abbr,
                Team = "Inter Milan",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                FirstName = "Giovani",
                LastName = "Lo Celso",
                DateOfBirth = new DateTime(1996, 4, 9, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 27,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Real Betis Balompié",
                League = "La Liga",
                Starting11 = false,
            },
        ];
    }

    /// <summary>
    /// Create a predefined list of 15 substitute players where each player has a fixed GUID identifier and full profile data.
    /// </summary>
    /// <returns>A list of 15 Player instances representing substitute players (squads 1–26); each entry includes a predefined Id (Guid) and populated fields such as name, date of birth, squad number, position (and abbreviation), team, league, and Starting11 set to false. Lo Celso (squad 27) is intentionally excluded — his squad number falls outside the seeded range so he can serve as the canonical Create/Delete fixture without conflicting with seeded data.</returns>
    public static List<Player> GetSubstitutesWithId()
    {
        return
        [
            new()
            {
                Id = Guid.Parse("5a9cd988-95e6-54c1-bc34-9aa08acca8d0"),
                FirstName = "Franco",
                MiddleName = "Daniel",
                LastName = "Armani",
                DateOfBirth = new DateTime(1986, 10, 16, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 1,
                Position = Position.Goalkeeper.Text,
                AbbrPosition = Position.Goalkeeper.Abbr,
                Team = "River Plate",
                League = "Copa de la Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("c62f2ac1-41e8-5d34-b073-2ba0913d0e31"),
                FirstName = "Gerónimo",
                LastName = "Rulli",
                DateOfBirth = new DateTime(1992, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 12,
                Position = Position.Goalkeeper.Text,
                AbbrPosition = Position.Goalkeeper.Abbr,
                Team = "Ajax Amsterdam",
                League = "Eredivisie",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("5fdb10e8-38c0-5084-9a3f-b369a960b9c2"),
                FirstName = "Juan",
                MiddleName = "Marcos",
                LastName = "Foyth",
                DateOfBirth = new DateTime(1998, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 2,
                Position = Position.RightBack.Text,
                AbbrPosition = Position.RightBack.Abbr,
                Team = "Villarreal",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("bbd441f7-fcfb-5834-8468-2a9004b64c8c"),
                FirstName = "Gonzalo",
                MiddleName = "Ariel",
                LastName = "Montiel",
                DateOfBirth = new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 4,
                Position = Position.RightBack.Text,
                AbbrPosition = Position.RightBack.Abbr,
                Team = "Nottingham Forest",
                League = "Premier League",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("d8bfea25-f189-5d5e-b3a5-ed89329b9f7c"),
                FirstName = "Germán",
                MiddleName = "Alejo",
                LastName = "Pezzella",
                DateOfBirth = new DateTime(1991, 6, 27, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 6,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "Real Betis Balompié",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("dca343a8-12e5-53d6-89a8-916b120a5ee4"),
                FirstName = "Marcos",
                MiddleName = "Javier",
                LastName = "Acuña",
                DateOfBirth = new DateTime(1991, 10, 28, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 8,
                Position = Position.LeftBack.Text,
                AbbrPosition = Position.LeftBack.Abbr,
                Team = "Sevilla FC",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("98306555-a466-5d18-804e-dc82175e697b"),
                FirstName = "Lisandro",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1998, 1, 18, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 25,
                Position = Position.CentreBack.Text,
                AbbrPosition = Position.CentreBack.Abbr,
                Team = "Manchester United",
                League = "Premier League",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("9d140400-196f-55d8-86e1-e0b96a375c83"),
                FirstName = "Leandro",
                MiddleName = "Daniel",
                LastName = "Paredes",
                DateOfBirth = new DateTime(1994, 6, 29, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 5,
                Position = Position.DefensiveMidfield.Text,
                AbbrPosition = Position.DefensiveMidfield.Abbr,
                Team = "AS Roma",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("d3b0e8e8-2c34-531a-b608-b24fed0ef986"),
                FirstName = "Exequiel",
                MiddleName = "Alejandro",
                LastName = "Palacios",
                DateOfBirth = new DateTime(1998, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 14,
                Position = Position.CentralMidfield.Text,
                AbbrPosition = Position.CentralMidfield.Abbr,
                Team = "Bayer 04 Leverkusen",
                League = "Bundesliga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("7cc8d527-56a2-58bd-9528-2618fc139d30"),
                FirstName = "Alejandro",
                MiddleName = "Darío",
                LastName = "Gómez",
                DateOfBirth = new DateTime(1988, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 17,
                Position = Position.LeftWinger.Text,
                AbbrPosition = Position.LeftWinger.Abbr,
                Team = "AC Monza",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("191c82af-0c51-526a-b903-c3600b61b506"),
                FirstName = "Guido",
                LastName = "Rodríguez",
                DateOfBirth = new DateTime(1994, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 18,
                Position = Position.DefensiveMidfield.Text,
                AbbrPosition = Position.DefensiveMidfield.Abbr,
                Team = "Real Betis Balompié",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("b1306b7b-a3a4-5f7c-90fd-dd5bdbed57ba"),
                FirstName = "Ángel",
                MiddleName = "Martín",
                LastName = "Correa",
                DateOfBirth = new DateTime(1995, 3, 9, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 15,
                Position = Position.RightWinger.Text,
                AbbrPosition = Position.RightWinger.Abbr,
                Team = "Atlético Madrid",
                League = "La Liga",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("ecec27e8-487b-5622-b116-0855020477ed"),
                FirstName = "Thiago",
                MiddleName = "Ezequiel",
                LastName = "Almada",
                DateOfBirth = new DateTime(2001, 4, 26, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 16,
                Position = Position.AttackingMidfield.Text,
                AbbrPosition = Position.AttackingMidfield.Abbr,
                Team = "Atlanta United FC",
                League = "Major League Soccer",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("7941cd7c-4df1-5952-97e8-1e7f5d08e8aa"),
                FirstName = "Paulo",
                MiddleName = "Exequiel",
                LastName = "Dybala",
                DateOfBirth = new DateTime(1993, 11, 15, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 21,
                Position = Position.SecondStriker.Text,
                AbbrPosition = Position.SecondStriker.Abbr,
                Team = "AS Roma",
                League = "Serie A",
                Starting11 = false,
            },
            new()
            {
                Id = Guid.Parse("79c96f29-c59f-5f98-96b8-3a5946246624"),
                FirstName = "Lautaro",
                MiddleName = "Javier",
                LastName = "Martínez",
                DateOfBirth = new DateTime(1997, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                SquadNumber = 22,
                Position = Position.CentreForward.Text,
                AbbrPosition = Position.CentreForward.Abbr,
                Team = "Inter Milan",
                League = "Serie A",
                Starting11 = false,
            },
        ];
    }

    /// <summary>
    /// The purpose of this method is to demonstrate the capabilities of
    /// System.Text.Json.JsonSerializer
    /// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization
    /// </summary>
    /// <returns>A list of Players.</returns>
    public static List<Player> MakeStarting11FromDeserializedJson()
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
                    "team": "SL Benfica",
                    "league": "Liga Portugal",
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
                    "team": "Brighton & Hove Albion",
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
                    "team": "Paris Saint-Germain",
                    "league": "Ligue 1",
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

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}
