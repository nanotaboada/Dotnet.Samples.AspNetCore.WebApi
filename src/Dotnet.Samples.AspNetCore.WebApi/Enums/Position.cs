namespace Dotnet.Samples.AspNetCore.WebApi.Enums;

public class Position : Enumeration
{
    public string Abbr { get; private set; }

    public static readonly Position Goalkeeper = new(1, "Goalkeeper", "GK");
    public static readonly Position RightBack = new(2, "Right-Back", "RB");
    public static readonly Position LeftBack = new(3, "Left-Back", "LB");
    public static readonly Position CentreBack = new(4, "Centre-Back", "CB");
    public static readonly Position DefensiveMidfield = new(5, "Defensive Midfield", "DM");
    public static readonly Position CentralMidfield = new(6, "Central Midfield", "CM");
    public static readonly Position RightWinger = new(7, "Right Winger", "RW");
    public static readonly Position AttackingMidfield = new(8, "Attacking Midfield", "AM");
    public static readonly Position CentreForward = new(9, "Centre-Forward", "CF");
    public static readonly Position SecondStriker = new(10, "Second Striker", "SS");
    public static readonly Position LeftWinger = new(11, "Left Winger", "LW");

    private Position(int id, string name, string abbr)
        : base(id, name) => Abbr = abbr;

    /// <summary>
    /// Returns a Position object based on the abbreviation.
    /// </summary>
    /// <remarks>
    /// This method searches through all the Position objects and returns the one
    /// that matches the provided abbreviation. If no match is found, it returns null.
    /// </remarks>
    /// <param name="abbr">The abbreviation of the Position.</param>
    /// <returns>
    /// A Position object if found; otherwise, null.
    /// </returns>
    public static Position? FromAbbr(string abbr) =>
        GetAll<Position>().FirstOrDefault(position => position.Abbr == abbr);

    /// <summary>
    /// Returns a Position object based on the ID.
    /// </summary>
    /// <remarks>
    /// This method searches through all the Position objects and returns the one
    /// that matches the provided ID. If no match is found, it returns null.
    /// </remarks>
    /// <param name="id">The ID of the Position.</param>
    /// <returns>
    /// A Position object if found; otherwise, null.
    /// </returns>
    public static Position? FromId(int id) =>
        GetAll<Position>().FirstOrDefault(position => position.Id == id);

    /// <summary>
    /// Checks if the provided abbreviation is valid.
    /// </summary>
    /// <remarks>
    /// This method checks if the provided abbreviation is not null or empty and
    /// if it corresponds to a valid Position object.
    /// </remarks>
    /// <param name="abbr">The abbreviation to check.</param>
    /// <returns>
    /// True if the abbreviation is valid; otherwise, false.
    /// </returns>
    public static bool IsValidAbbr(string? abbr) =>
        !string.IsNullOrWhiteSpace(abbr) && FromAbbr(abbr!) is not null;
}
