namespace Dotnet.Samples.AspNetCore.WebApi.Models;

/// <summary>
/// Represents one page of a paginated result.
/// </summary>
/// <typeparam name="T">
/// The type of item contained in the page.
/// </typeparam>
public sealed class PagedResponse<T>
{
    /// <summary>
    /// Items contained in the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Maximum number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of matching items.
    /// </summary>
    public int TotalCount { get; set; }
}