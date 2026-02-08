using System.ComponentModel.DataAnnotations;

namespace Dotnet.Samples.AspNetCore.WebApi.Models;

/// <summary>
/// Model for Player response.
/// </summary>
/// <remarks>
/// This class is used to send Player data to the client.
/// </remarks>
public class PlayerResponseModel
{
    /// <summary>
    /// The full name of the Player (combined first, middle, and last names).
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// The formatted birth date of the Player.
    /// </summary>
    public string? Birth { get; set; }

    /// <summary>
    /// The squad number (dorsal) of the Player.
    /// </summary>
    public int Dorsal { get; set; }

    /// <summary>
    /// The playing position of the Player.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The club (team) to which the Player belongs.
    /// </summary>
    public string? Club { get; set; }

    /// <summary>
    /// The league where the club plays.
    /// </summary>
    public string? League { get; set; }

    /// <summary>
    /// Indicates whether the Player is in the starting 11 (formatted as string).
    /// </summary>
    public string? Starting11 { get; set; }
}
