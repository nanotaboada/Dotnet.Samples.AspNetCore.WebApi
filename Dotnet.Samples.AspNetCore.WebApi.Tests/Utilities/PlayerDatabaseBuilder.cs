using System;
using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests
{
    public static class PlayerDatabaseBuilder
    {
        public static (DbConnection, DbContextOptions<PlayerContext>) BuildDatabase()
        {
            var dbConnection = new SqliteConnection("Filename=:memory:");
            dbConnection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<PlayerContext>()
                .UseSqlite(dbConnection)
                .Options;

            return (dbConnection, dbContextOptions);
        }

        public static void CreateDatabase(PlayerContext context)
        {
            using var dbCommand = context.Database.GetDbConnection().CreateCommand();

            dbCommand.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS players
                (
                    id	            INTEGER,
                    firstName	    TEXT NOT NULL,
                    middleName      TEXT,
                    lastName	    TEXT NOT NULL,
                    dateOfBirth	    TEXT,
                    squadNumber	    INTEGER NOT NULL,
                    position        TEXT NOT NULL,
                    abbrPosition    TEXT,
                    team            TEXT,
                    league    	    TEXT,
                    starting11      BOOLEAN,
                    PRIMARY KEY(id)
                );";

            dbCommand.ExecuteNonQuery();
        }

        public static void Seed(PlayerContext context)
        {
            context.AddRange(PlayerDataBuilder.SeedWithDeserializedJson());
            context.SaveChanges();
        }
    }
}
