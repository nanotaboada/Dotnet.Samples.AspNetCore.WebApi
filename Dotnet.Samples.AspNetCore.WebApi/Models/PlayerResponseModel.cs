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
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string? Birth { get; set; }

    public int Dorsal { get; set; }

    public string? Position { get; set; }

    public string? Club { get; set; }

    public string? League { get; set; }

    public string? Starting11 { get; set; }
}
