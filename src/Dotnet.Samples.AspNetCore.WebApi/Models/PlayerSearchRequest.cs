namespace Dotnet.Samples.AspNetCore.WebApi.Models;

/// <summary>
/// Represents the query parameters used to search
/// and paginate players.
/// </summary>
public sealed class PlayerSearchRequest
{
    /// <summary>
    /// Optional team filter.
    /// </summary>
    public string? Team { get; set; }

    /// <summary>
    /// Optional abbreviated position filter,
    /// such as GK, CB, CM, or CF.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The requested page number.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of players returned per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}