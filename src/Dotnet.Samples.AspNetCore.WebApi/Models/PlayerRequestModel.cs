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
    public long Id { get; set; }

    [Required]
    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required]
    public int SquadNumber { get; set; }

    [Required]
    public string? AbbrPosition { get; set; }

    public string? Team { get; set; }

    public string? League { get; set; }
}
