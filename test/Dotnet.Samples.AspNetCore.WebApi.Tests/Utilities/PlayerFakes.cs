using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Utilities;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;

/// <summary>
/// Test data factory for Player entities.
/// Wraps production data from PlayerData with test-specific modifications (e.g., GUID assignment).
/// A Fake is a working implementation that's simpler than the real one.
/// It usually has some "real" logic but is not suitable for production
/// (e.g., an in‑memory database instead of a full SQL Server). Fakes are
/// useful when you need behavior that's closer to reality but still want
/// to avoid external dependencies.
/// </summary>
public static class PlayerFakes
{
    /// <summary>
    /// Returns the starting 11 players with generated GUIDs for in-memory testing.
    /// Reuses production player data from PlayerData.MakeStarting11().
    /// </summary>
    public static List<Player> MakeStarting11()
    {
        var players = PlayerData.MakeStarting11();

        // Assign GUIDs for in-memory database testing
        foreach (var player in players)
        {
            player.Id = Guid.NewGuid();
        }

        return players;
    }

    /// <summary>
    /// Returns a specific player from the starting 11 by squad number.
    /// Reuses production player data from PlayerData.MakeStarting11().
    /// </summary>
    public static Player MakeFromStarting11(int squadNumber)
    {
        var player =
            PlayerData.MakeStarting11().SingleOrDefault(p => p.SquadNumber == squadNumber)
            ?? throw new ArgumentNullException(
                $"Player with Squad Number {squadNumber} not found."
            );

        player.Id = Guid.NewGuid();
        return player;
    }

    /// <summary>
    /// Returns a new player (substitute) for testing create operations.
    /// Reuses production player data from PlayerData.GetSubstitutes().
    /// </summary>
    public static Player MakeNew()
    {
        // Get Leandro Paredes (squad number 5) from substitutes
        var player =
            PlayerData.GetSubstitutes().SingleOrDefault(player => player.SquadNumber == 5)
            ?? throw new InvalidOperationException(
                "Substitute player with squad number 5 not found."
            );

        player.Id = Guid.NewGuid();
        return player;
    }

    /* -------------------------------------------------------------------------
     * Create
     * ---------------------------------------------------------------------- */

    public static PlayerRequestModel MakeRequestModelForCreate()
    {
        var player = MakeNew();

        return new PlayerRequestModel()
        {
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

    public static PlayerRequestModel MakeRequestModelForRetrieve(int squadNumber)
    {
        var player =
            PlayerData.MakeStarting11().SingleOrDefault(p => p.SquadNumber == squadNumber)
            ?? throw new ArgumentNullException(
                $"Player with Squad Number {squadNumber} not found."
            );

        return new PlayerRequestModel
        {
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

    public static PlayerResponseModel MakeResponseModelForRetrieve(int squadNumber)
    {
        var player =
            PlayerData.MakeStarting11().SingleOrDefault(p => p.SquadNumber == squadNumber)
            ?? throw new ArgumentNullException(
                $"Player with Squad Number {squadNumber} not found."
            );

        return new PlayerResponseModel
        {
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
            .. PlayerData
                .MakeStarting11()
                .Select(player =>
                {
                    player.Id = Guid.NewGuid();
                    return new PlayerResponseModel
                    {
                        FullName =
                            $"{player.FirstName} {(string.IsNullOrWhiteSpace(player.MiddleName) ? "" : player.MiddleName + " ")}{player.LastName}".Trim(),
                        Birth = $"{player.DateOfBirth:MMMM d, yyyy}",
                        Dorsal = player.SquadNumber,
                        Position = player.Position,
                        Club = player.Team,
                        League = player.League,
                        Starting11 = player.Starting11 ? "Yes" : "No"
                    };
                })
        ];

    /* -------------------------------------------------------------------------
     * Update
     * ---------------------------------------------------------------------- */

    public static PlayerRequestModel MakeRequestModelForUpdate(int squadNumber)
    {
        return MakeRequestModelForRetrieve(squadNumber);
    }

    public static PlayerResponseModel MakeResponseModelForUpdate(int squadNumber)
    {
        return MakeResponseModelForRetrieve(squadNumber);
    }
}
