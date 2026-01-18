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
        /// <summary>
        /// Creates an in-memory SQLite connection and DbContext options for testing.
        /// The connection remains open for the lifetime of the test.
        /// </summary>
        /// <returns>A tuple containing the SQLite connection and DbContext options.</returns>
        public static (DbConnection, DbContextOptions<PlayerDbContext>) CreateSqliteConnection()
        {
            var dbConnection = new SqliteConnection("Filename=:memory:");
            dbConnection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<PlayerDbContext>()
                .UseSqlite(dbConnection)
                .Options;

            return (dbConnection, dbContextOptions);
        }

        /// <summary>
        /// Creates a PlayerDbContext instance with the specified options.
        /// </summary>
        /// <param name="dbContextOptions">The DbContext options to use.</param>
        /// <returns>A new PlayerDbContext instance.</returns>
        public static PlayerDbContext CreateDbContext(
            DbContextOptions<PlayerDbContext> dbContextOptions
        )
        {
            return new PlayerDbContext(dbContextOptions);
        }

        /// <summary>
        /// Creates the database schema for the test database.
        /// Extension method for PlayerDbContext.
        /// </summary>
        /// <param name="context">The PlayerDbContext instance.</param>
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

        /// <summary>
        /// Seeds the test database with the starting 11 players.
        /// Extension method for PlayerDbContext.
        /// </summary>
        /// <param name="context">The PlayerDbContext instance.</param>
        public static void Seed(this PlayerDbContext context)
        {
            context.Players.AddRange(PlayerFakes.MakeStarting11());
            context.SaveChanges();
        }
    }
}
