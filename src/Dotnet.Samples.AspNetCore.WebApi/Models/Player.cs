using System.ComponentModel.DataAnnotations;

namespace Dotnet.Samples.AspNetCore.WebApi.Models;

/// <summary>
/// Model for Player entity.
/// </summary>
/// <remarks>
/// This class represents the Player entity in the database.
/// </remarks>
public class Player
{
    /// <summary>
    /// The unique identifier for the Player.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The first name of the Player.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The middle name of the Player, if any.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// The last name of the Player.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// The date of birth of the Player.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// The squad number assigned to the Player.
    /// </summary>
    public int SquadNumber { get; set; }

    /// <summary>
    /// The playing position of the Player.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The abbreviated form of the Player's position.
    /// </summary>
    public string? AbbrPosition { get; set; }

    /// <summary>
    /// The team to which the Player belongs.
    /// </summary>
    public string? Team { get; set; }

    /// <summary>
    /// The league where the team plays.
    /// </summary>
    public string? League { get; set; }

    /// <summary>
    /// Indicates whether the Player is in the starting 11.
    /// </summary>
    public bool Starting11 { get; set; }
}
