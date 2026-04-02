using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class NormalizePlayerDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update UUIDs to canonical UUID v5 values for starting 11
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '01772c59-43f0-5d85-b913-c78e4e281452' WHERE SquadNumber = 23"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'da31293b-4c7e-5e0f-a168-469ee29ecbc4' WHERE SquadNumber = 26"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'c096c69e-762b-5281-9290-bb9c167a24a0' WHERE SquadNumber = 13"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd5f7dd7a-1dcb-5960-ba27-e34865b63358' WHERE SquadNumber = 19"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '2f6f90a0-9b9d-5023-96d2-a2aaf03143a6' WHERE SquadNumber = 3"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'b5b46e79-929e-5ed2-949d-0d167109c022' WHERE SquadNumber = 11"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '0293b282-1da8-562e-998e-83849b417a42' WHERE SquadNumber = 7"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd3ba552a-dac3-588a-b961-1ea7224017fd', Team = 'SL Benfica', League = 'Liga Portugal' WHERE SquadNumber = 24"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '9613cae9-16ab-5b54-937e-3135123b9e0d', Team = 'Brighton & Hove Albion' WHERE SquadNumber = 20"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'acc433bf-d505-51fe-831e-45eb44c4d43c', Team = 'Paris Saint-Germain', League = 'Ligue 1' WHERE SquadNumber = 10"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '38bae91d-8519-55a2-b30a-b9fe38849bfb' WHERE SquadNumber = 9"
            );

            // Update UUIDs to canonical UUID v5 values for substitutes
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '5a9cd988-95e6-54c1-bc34-9aa08acca8d0' WHERE SquadNumber = 1"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'c62f2ac1-41e8-5d34-b073-2ba0913d0e31' WHERE SquadNumber = 12"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '5fdb10e8-38c0-5084-9a3f-b369a960b9c2' WHERE SquadNumber = 2"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'bbd441f7-fcfb-5834-8468-2a9004b64c8c' WHERE SquadNumber = 4"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd8bfea25-f189-5d5e-b3a5-ed89329b9f7c' WHERE SquadNumber = 6"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'dca343a8-12e5-53d6-89a8-916b120a5ee4' WHERE SquadNumber = 8"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '98306555-a466-5d18-804e-dc82175e697b' WHERE SquadNumber = 25"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '9d140400-196f-55d8-86e1-e0b96a375c83' WHERE SquadNumber = 5"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd3b0e8e8-2c34-531a-b608-b24fed0ef986' WHERE SquadNumber = 14"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '7cc8d527-56a2-58bd-9528-2618fc139d30' WHERE SquadNumber = 17"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '191c82af-0c51-526a-b903-c3600b61b506' WHERE SquadNumber = 18"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'b1306b7b-a3a4-5f7c-90fd-dd5bdbed57ba' WHERE SquadNumber = 15"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'ecec27e8-487b-5622-b116-0855020477ed' WHERE SquadNumber = 16"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '7941cd7c-4df1-5952-97e8-1e7f5d08e8aa' WHERE SquadNumber = 21"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '79c96f29-c59f-5f98-96b8-3a5946246624' WHERE SquadNumber = 22"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert team/league corrections
            migrationBuilder.Sql(
                "UPDATE Players SET Team = 'Chelsea FC', League = 'Premier League' WHERE SquadNumber = 24"
            );
            migrationBuilder.Sql("UPDATE Players SET Team = 'Liverpool FC' WHERE SquadNumber = 20");
            migrationBuilder.Sql(
                "UPDATE Players SET Team = 'Inter Miami CF', League = 'Major League Soccer' WHERE SquadNumber = 10"
            );

            // Revert UUIDs for starting 11
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'f91b9671-cfd1-48d7-afb9-34e93568c9ee' WHERE SquadNumber = 23"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '51ec988a-0d8b-42d9-84e4-5e17c93d8d50' WHERE SquadNumber = 26"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '0969be24-086c-4c51-9c29-0280683b8133' WHERE SquadNumber = 13"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'ac532709-4682-49db-acc2-395f61f405ab' WHERE SquadNumber = 19"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'dc052ee4-c69d-49da-a256-b8ec727f4d59' WHERE SquadNumber = 3"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '6def9bb7-23c2-42b5-b37b-2e9b6fec31cd' WHERE SquadNumber = 11"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '8ca911d9-ab32-4366-b2b1-cad5eb6f4bcc' WHERE SquadNumber = 7"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '198c4774-9607-4e76-8475-ec2528af69d2' WHERE SquadNumber = 24"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '06971ada-1b3d-4d4a-88f5-e2f35311b5aa' WHERE SquadNumber = 20"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'df6f6bab-5efd-4518-80bb-09ef54435636' WHERE SquadNumber = 10"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = '27cf4e29-67d5-4c3b-9cf8-7d3fa3942fcb' WHERE SquadNumber = 9"
            );

            // Revert UUIDs for substitutes
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'b1f8a5d3-2c4e-4a6b-8d9f-1e3c5a7b9d2f' WHERE SquadNumber = 1"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'c2e9b6f4-3d5f-4b7c-9e0a-2f4d6b8c0e3a' WHERE SquadNumber = 12"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd3f0c7e5-4e6a-5c8d-0f1b-3a5e7c9d1f4b' WHERE SquadNumber = 2"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'e4a1d8f6-5f7b-6d9e-1a2c-4b6d8e0a2c5d' WHERE SquadNumber = 4"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'f5b2e9a7-6a8c-7e0f-2b3d-5c7e9a1b3d6e' WHERE SquadNumber = 6"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'a6c3f0b8-7b9d-8f1a-3c4e-6d8f0b2c4e7f' WHERE SquadNumber = 8"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'b7d4a1c9-8c0e-9a2b-4d5f-7e9a1c3d5f8a' WHERE SquadNumber = 25"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'c8e5b2d0-9d1f-0b3c-5e6a-8f0b2d4e6a9b' WHERE SquadNumber = 5"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd9f6c3e1-0e2a-1c4d-6f7b-9a1c3e5f7b0c' WHERE SquadNumber = 14"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'e0a7d4f2-1f3b-2d5e-7a8c-0b2d4f6a8c1d' WHERE SquadNumber = 17"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'f1b8e5a3-2a4c-3e6f-8b9d-1c3e5a7b9d2e' WHERE SquadNumber = 18"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'a2c9f6b4-3b5d-4f7a-9c0e-2d4f6b8c0e3f' WHERE SquadNumber = 15"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'b3d0a7c5-4c6e-5a8b-0d1f-3e5a7c9d1f4a' WHERE SquadNumber = 16"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'c4e1b8d6-5d7f-6b9c-1e2a-4f6d8e0a2c5b' WHERE SquadNumber = 21"
            );
            migrationBuilder.Sql(
                "UPDATE Players SET Id = 'd5f2c9e7-6e8a-7c0d-2f3b-5a7e9a1b3d6c' WHERE SquadNumber = 22"
            );
        }
    }
}
