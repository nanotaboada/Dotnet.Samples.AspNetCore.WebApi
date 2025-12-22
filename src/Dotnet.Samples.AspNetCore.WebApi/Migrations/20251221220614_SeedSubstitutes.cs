using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedSubstitutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var substitutes = PlayerData.GetSubstitutesWithId();

            foreach (var player in substitutes)
            {
                migrationBuilder.InsertData(
                    table: "Players",
                    columns:
                    [
                        "Id",
                        "FirstName",
                        "MiddleName",
                        "LastName",
                        "DateOfBirth",
                        "SquadNumber",
                        "Position",
                        "AbbrPosition",
                        "Team",
                        "League",
                        "Starting11"
                    ],
                    values: new object[]
                    {
                        player.Id,
                        player.FirstName,
                        player.MiddleName,
                        player.LastName,
                        player.DateOfBirth,
                        player.SquadNumber,
                        player.Position,
                        player.AbbrPosition,
                        player.Team,
                        player.League,
                        player.Starting11
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var substitutes = PlayerData.GetSubstitutesWithId();
            foreach (var player in substitutes)
            {
                migrationBuilder.DeleteData(table: "Players", keyColumn: "Id", keyValue: player.Id);
            }
        }
    }
}
