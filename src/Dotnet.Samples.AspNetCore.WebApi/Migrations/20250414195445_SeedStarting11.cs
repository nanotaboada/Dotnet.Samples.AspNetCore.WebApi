using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedStarting11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var starting11 = PlayerData.MakeStarting11WithId();

            foreach (var player in starting11)
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
            var starting11 = PlayerData.MakeStarting11WithId();
            foreach (var player in starting11)
            {
                migrationBuilder.DeleteData(table: "Players", keyColumn: "Id", keyValue: player.Id);
            }
        }
    }
}
