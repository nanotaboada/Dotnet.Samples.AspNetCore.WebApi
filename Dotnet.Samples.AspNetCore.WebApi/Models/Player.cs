using System.ComponentModel.DataAnnotations;

namespace Dotnet.Samples.AspNetCore.WebApi.Models;

public class Player
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
    public string? Position { get; set; }

    public string? AbbrPosition { get; set; }

    public string? Team { get; set; }

    public string? League { get; set; }

    public bool Starting11 { get; set; }
}
