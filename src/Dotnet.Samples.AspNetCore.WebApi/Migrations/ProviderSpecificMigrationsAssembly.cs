using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
#pragma warning disable EF1001 // Internal EF Core API usage.
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace Dotnet.Samples.AspNetCore.WebApi.Migrations;

/// <summary>
/// Filters the available migrations to only those belonging to the active database provider's
/// namespace, ensuring SQLite and PostgreSQL each apply their own migration set.
/// </summary>
#pragma warning disable CS9107 // currentContext is intentionally captured by both the subclass and the base class
public class ProviderSpecificMigrationsAssembly(
    ICurrentDbContext currentContext,
    IDbContextOptions options,
    IMigrationsIdGenerator idGenerator,
    IDiagnosticsLogger<DbLoggerCategory.Migrations> logger
) : MigrationsAssembly(currentContext, options, idGenerator, logger)
#pragma warning restore CS9107
{
    private const string SqliteNamespace = "Dotnet.Samples.AspNetCore.WebApi.Migrations";
    private const string NpgsqlNamespace = "Dotnet.Samples.AspNetCore.WebApi.Migrations.Npgsql";

    public override IReadOnlyDictionary<string, TypeInfo> Migrations
    {
        get
        {
            var providerName = currentContext.Context.Database.ProviderName ?? string.Empty;
            var targetNamespace = providerName.Contains(
                "Sqlite",
                StringComparison.OrdinalIgnoreCase
            )
                ? SqliteNamespace
                : NpgsqlNamespace;

            return new SortedDictionary<string, TypeInfo>(
                base.Migrations.Where(m => m.Value.Namespace == targetNamespace)
                    .ToDictionary(m => m.Key, m => m.Value)
            );
        }
    }
}
#pragma warning restore EF1001
