using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations.Npgsql
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    SquadNumber = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: true),
                    AbbrPosition = table.Column<string>(type: "text", nullable: true),
                    Team = table.Column<string>(type: "text", nullable: true),
                    League = table.Column<string>(type: "text", nullable: true),
                    Starting11 = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Players_SquadNumber",
                table: "Players",
                column: "SquadNumber",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Players");
        }
    }
}
