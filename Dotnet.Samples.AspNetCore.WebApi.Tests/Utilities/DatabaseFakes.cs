using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities
{
    /// <summary>
    /// A Fake is a working implementation that’s simpler than the real one.
    /// It usually has some “real” logic but is not suitable for production
    /// (e.g., an in‑memory database instead of a full SQL Server). Fakes are
    /// useful when you need behavior that’s closer to reality but still want
    /// to avoid external dependencies.
    /// </summary>
    public static class DatabaseFakes
    {
        public static (DbConnection, DbContextOptions<PlayerDbContext>) CreateSqliteConnection()
        {
            var dbConnection = new SqliteConnection("Filename=:memory:");
            dbConnection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<PlayerDbContext>()
                .UseSqlite(dbConnection)
                .Options;

            return (dbConnection, dbContextOptions);
        }

        public static PlayerDbContext CreateDbContext(
            DbContextOptions<PlayerDbContext> dbContextOptions
        )
        {
            return new PlayerDbContext(dbContextOptions);
        }

        public static void CreateTable(this PlayerDbContext context)
        {
            using var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText =
                @"
                CREATE TABLE players (
                    id INTEGER PRIMARY KEY,
                    firstName TEXT NOT NULL,
                    /* ... other columns ... */
                )";
            cmd.ExecuteNonQuery();
        }

        public static void Seed(this PlayerDbContext context)
        {
            context.Players.AddRange(PlayerFakes.GetStarting11());
            context.SaveChanges();
        }
    }
}
