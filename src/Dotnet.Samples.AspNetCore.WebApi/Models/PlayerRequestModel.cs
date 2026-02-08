using System.ComponentModel.DataAnnotations;

namespace Dotnet.Samples.AspNetCore.WebApi.Models;

/// <summary>
/// Model for Player request.
/// </summary>
/// <remarks>
/// This class is used to receive Player data from the client.
/// The properties are decorated with validation attributes to ensure that
/// the required fields are provided and that the data is in the correct format.
/// </remarks>
public class PlayerRequestModel
{
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
    /// The squad number assigned to the Player (required).
    /// </summary>
    public required int SquadNumber { get; set; }

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
}
