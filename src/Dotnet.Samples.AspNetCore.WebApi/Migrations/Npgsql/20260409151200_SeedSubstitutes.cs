using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations.Npgsql
{
    /// <inheritdoc />
    public partial class SeedSubstitutes : Migration
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
                        new Guid("191c82af-0c51-526a-b903-c3600b61b506"),
                        "DM",
                        new DateTime(1994, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Guido",
                        "Rodríguez",
                        "La Liga",
                        null,
                        "Defensive Midfield",
                        18,
                        false,
                        "Real Betis Balompié"
                    },
                    {
                        new Guid("5a9cd988-95e6-54c1-bc34-9aa08acca8d0"),
                        "GK",
                        new DateTime(1986, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Franco",
                        "Armani",
                        "Copa de la Liga",
                        "Daniel",
                        "Goalkeeper",
                        1,
                        false,
                        "River Plate"
                    },
                    {
                        new Guid("5fdb10e8-38c0-5084-9a3f-b369a960b9c2"),
                        "RB",
                        new DateTime(1998, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Juan",
                        "Foyth",
                        "La Liga",
                        "Marcos",
                        "Right-Back",
                        2,
                        false,
                        "Villarreal"
                    },
                    {
                        new Guid("7941cd7c-4df1-5952-97e8-1e7f5d08e8aa"),
                        "SS",
                        new DateTime(1993, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Paulo",
                        "Dybala",
                        "Serie A",
                        "Exequiel",
                        "Second Striker",
                        21,
                        false,
                        "AS Roma"
                    },
                    {
                        new Guid("79c96f29-c59f-5f98-96b8-3a5946246624"),
                        "CF",
                        new DateTime(1997, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Lautaro",
                        "Martínez",
                        "Serie A",
                        "Javier",
                        "Centre-Forward",
                        22,
                        false,
                        "Inter Milan"
                    },
                    {
                        new Guid("7cc8d527-56a2-58bd-9528-2618fc139d30"),
                        "LW",
                        new DateTime(1988, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Alejandro",
                        "Gómez",
                        "Serie A",
                        "Darío",
                        "Left Winger",
                        17,
                        false,
                        "AC Monza"
                    },
                    {
                        new Guid("98306555-a466-5d18-804e-dc82175e697b"),
                        "CB",
                        new DateTime(1998, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Lisandro",
                        "Martínez",
                        "Premier League",
                        null,
                        "Centre-Back",
                        25,
                        false,
                        "Manchester United"
                    },
                    {
                        new Guid("9d140400-196f-55d8-86e1-e0b96a375c83"),
                        "DM",
                        new DateTime(1994, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Leandro",
                        "Paredes",
                        "Serie A",
                        "Daniel",
                        "Defensive Midfield",
                        5,
                        false,
                        "AS Roma"
                    },
                    {
                        new Guid("b1306b7b-a3a4-5f7c-90fd-dd5bdbed57ba"),
                        "RW",
                        new DateTime(1995, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Ángel",
                        "Correa",
                        "La Liga",
                        "Martín",
                        "Right Winger",
                        15,
                        false,
                        "Atlético Madrid"
                    },
                    {
                        new Guid("bbd441f7-fcfb-5834-8468-2a9004b64c8c"),
                        "RB",
                        new DateTime(1997, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Gonzalo",
                        "Montiel",
                        "Premier League",
                        "Ariel",
                        "Right-Back",
                        4,
                        false,
                        "Nottingham Forest"
                    },
                    {
                        new Guid("c62f2ac1-41e8-5d34-b073-2ba0913d0e31"),
                        "GK",
                        new DateTime(1992, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Gerónimo",
                        "Rulli",
                        "Eredivisie",
                        null,
                        "Goalkeeper",
                        12,
                        false,
                        "Ajax Amsterdam"
                    },
                    {
                        new Guid("d3b0e8e8-2c34-531a-b608-b24fed0ef986"),
                        "CM",
                        new DateTime(1998, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Exequiel",
                        "Palacios",
                        "Bundesliga",
                        "Alejandro",
                        "Central Midfield",
                        14,
                        false,
                        "Bayer 04 Leverkusen"
                    },
                    {
                        new Guid("d8bfea25-f189-5d5e-b3a5-ed89329b9f7c"),
                        "CB",
                        new DateTime(1991, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Germán",
                        "Pezzella",
                        "La Liga",
                        "Alejo",
                        "Centre-Back",
                        6,
                        false,
                        "Real Betis Balompié"
                    },
                    {
                        new Guid("dca343a8-12e5-53d6-89a8-916b120a5ee4"),
                        "LB",
                        new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Marcos",
                        "Acuña",
                        "La Liga",
                        "Javier",
                        "Left-Back",
                        8,
                        false,
                        "Sevilla FC"
                    },
                    {
                        new Guid("ecec27e8-487b-5622-b116-0855020477ed"),
                        "AM",
                        new DateTime(2001, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Thiago",
                        "Almada",
                        "Major League Soccer",
                        "Ezequiel",
                        "Attacking Midfield",
                        16,
                        false,
                        "Atlanta United FC"
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
                keyValue: new Guid("191c82af-0c51-526a-b903-c3600b61b506")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("5a9cd988-95e6-54c1-bc34-9aa08acca8d0")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("5fdb10e8-38c0-5084-9a3f-b369a960b9c2")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("7941cd7c-4df1-5952-97e8-1e7f5d08e8aa")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("79c96f29-c59f-5f98-96b8-3a5946246624")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("7cc8d527-56a2-58bd-9528-2618fc139d30")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("98306555-a466-5d18-804e-dc82175e697b")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("9d140400-196f-55d8-86e1-e0b96a375c83")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("b1306b7b-a3a4-5f7c-90fd-dd5bdbed57ba")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("bbd441f7-fcfb-5834-8468-2a9004b64c8c")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("c62f2ac1-41e8-5d34-b073-2ba0913d0e31")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("d3b0e8e8-2c34-531a-b608-b24fed0ef986")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("d8bfea25-f189-5d5e-b3a5-ed89329b9f7c")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("dca343a8-12e5-53d6-89a8-916b120a5ee4")
            );

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("ecec27e8-487b-5622-b116-0855020477ed")
            );
        }
    }
}
