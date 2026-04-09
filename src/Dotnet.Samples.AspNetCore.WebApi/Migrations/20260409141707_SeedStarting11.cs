using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedStarting11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[]
                {
                    "Id",
                    "AbbrPosition",
                    "DateOfBirth",
                    "FirstName",
                    "LastName",
                    "League",
                    "MiddleName",
                    "Position",
                    "SquadNumber",
                    "Starting11",
                    "Team"
                },
                values: new object[,]
                {
                    {
                        new Guid("01772c59-43f0-5d85-b913-c78e4e281452"),
                        "GK",
                        new DateTime(1992, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Damián",
                        "Martínez",
                        "Premier League",
                        "Emiliano",
                        "Goalkeeper",
                        23,
                        true,
                        "Aston Villa FC"
                    },
                    {
                        new Guid("0293b282-1da8-562e-998e-83849b417a42"),
                        "CM",
                        new DateTime(1994, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Rodrigo",
                        "de Paul",
                        "La Liga",
                        "Javier",
                        "Central Midfield",
                        7,
                        true,
                        "Altético Madrid"
                    },
                    {
                        new Guid("2f6f90a0-9b9d-5023-96d2-a2aaf03143a6"),
                        "LB",
                        new DateTime(1992, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Nicolás",
                        "Tagliafico",
                        "Ligue 1",
                        "Alejandro",
                        "Left-Back",
                        3,
                        true,
                        "Olympique Lyon"
                    },
                    {
                        new Guid("38bae91d-8519-55a2-b30a-b9fe38849bfb"),
                        "CF",
                        new DateTime(2000, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Julián",
                        "Álvarez",
                        "Premier League",
                        null,
                        "Centre-Forward",
                        9,
                        true,
                        "Manchester City"
                    },
                    {
                        new Guid("9613cae9-16ab-5b54-937e-3135123b9e0d"),
                        "CM",
                        new DateTime(1998, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Alexis",
                        "Mac Allister",
                        "Premier League",
                        null,
                        "Central Midfield",
                        20,
                        true,
                        "Brighton & Hove Albion"
                    },
                    {
                        new Guid("acc433bf-d505-51fe-831e-45eb44c4d43c"),
                        "RW",
                        new DateTime(1987, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Lionel",
                        "Messi",
                        "Ligue 1",
                        "Andrés",
                        "Right Winger",
                        10,
                        true,
                        "Paris Saint-Germain"
                    },
                    {
                        new Guid("b5b46e79-929e-5ed2-949d-0d167109c022"),
                        "RW",
                        new DateTime(1988, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Ángel",
                        "Di María",
                        "Liga Portugal",
                        "Fabián",
                        "Right Winger",
                        11,
                        true,
                        "SL Benfica"
                    },
                    {
                        new Guid("c096c69e-762b-5281-9290-bb9c167a24a0"),
                        "CB",
                        new DateTime(1998, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Cristian",
                        "Romero",
                        "Premier League",
                        "Gabriel",
                        "Centre-Back",
                        13,
                        true,
                        "Tottenham Hotspur"
                    },
                    {
                        new Guid("d3ba552a-dac3-588a-b961-1ea7224017fd"),
                        "CM",
                        new DateTime(2001, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Enzo",
                        "Fernández",
                        "Liga Portugal",
                        "Jeremías",
                        "Central Midfield",
                        24,
                        true,
                        "SL Benfica"
                    },
                    {
                        new Guid("d5f7dd7a-1dcb-5960-ba27-e34865b63358"),
                        "CB",
                        new DateTime(1988, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Nicolás",
                        "Otamendi",
                        "Liga Portugal",
                        "Hernán Gonzalo",
                        "Centre-Back",
                        19,
                        true,
                        "SL Benfica"
                    },
                    {
                        new Guid("da31293b-4c7e-5e0f-a168-469ee29ecbc4"),
                        "RB",
                        new DateTime(1998, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Nahuel",
                        "Molina",
                        "La Liga",
                        null,
                        "Right-Back",
                        26,
                        true,
                        "Altético Madrid"
                    }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("01772c59-43f0-5d85-b913-c78e4e281452")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("0293b282-1da8-562e-998e-83849b417a42")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("2f6f90a0-9b9d-5023-96d2-a2aaf03143a6")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("38bae91d-8519-55a2-b30a-b9fe38849bfb")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("9613cae9-16ab-5b54-937e-3135123b9e0d")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("acc433bf-d505-51fe-831e-45eb44c4d43c")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("b5b46e79-929e-5ed2-949d-0d167109c022")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("c096c69e-762b-5281-9290-bb9c167a24a0")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("d3ba552a-dac3-588a-b961-1ea7224017fd")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("d5f7dd7a-1dcb-5960-ba27-e34865b63358")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("da31293b-4c7e-5e0f-a168-469ee29ecbc4")
            );
        }
    }
}
