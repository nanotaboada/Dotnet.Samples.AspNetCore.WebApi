# 0014. Configurable Database Provider

Date: 2026-05-02

## Status

Accepted — supersedes [ADR-0003](0003-use-sqlite-for-data-storage.md)

## Context

The project used SQLite as its only database engine (ADR-0003). This meant all environments — local development, Docker Compose, and any deployment — ran the same SQLite file-based database. For the majority of contributors this is ideal: zero infrastructure required.

However, a fixed SQLite-only setup has limits:

- SQLite does not support concurrent writes, so multi-instance deployments are not possible.
- Developers who want to validate production-parity behaviour (e.g. PostgreSQL-specific query plans, type semantics, or constraint handling) have no path to do so without modifying the project.
- A fixed SQLite-in-dev / PostgreSQL-in-prod split (a common alternative) introduces a different problem: subtle behavioral differences between environments that are hard to catch locally.

The goal is to make the database engine fully configurable with a single environment variable, so the same stack is used consistently across every environment a developer chooses to run.

## Decision

We will introduce a `DATABASE_PROVIDER` environment variable that selects the database engine at startup:

- **`DATABASE_PROVIDER=sqlite`** (default): SQLite everywhere. Zero infrastructure required. Works on any machine without Docker. Clone and run.
- **`DATABASE_PROVIDER=postgres`**: PostgreSQL everywhere. Requires Docker. Opt-in for developers who want a server-based engine or full production parity.

The default is `sqlite` to keep the barrier to entry as low as possible.

### Provider selection at startup

`ServiceCollectionExtensions.AddDbContextPool` reads `DATABASE_PROVIDER` and wires the appropriate EF Core provider:

```csharp
switch (provider.ToLowerInvariant())
{
    case "postgres":
        options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"));
        break;
    default:
        options.UseSqlite($"Data Source={dataSource}");
        break;
}
```

### Provider-specific EF Core migrations

SQLite and PostgreSQL require different column type mappings (`TEXT`/`INTEGER` vs `uuid`/`boolean`/`timestamp with time zone`). A single migration set cannot satisfy both providers. To resolve this:

- SQLite migrations remain in `Migrations/` (namespace `...Migrations`).
- PostgreSQL migrations are placed in `Migrations/Npgsql/` (namespace `...Migrations.Npgsql`).
- A custom `ProviderSpecificMigrationsAssembly` (implementing EF Core's `IMigrationsAssembly`) filters the discovered migrations to only those in the active provider's namespace. It is registered via `options.ReplaceService<IMigrationsAssembly, ProviderSpecificMigrationsAssembly>()`.

`MigrateAsync()` at startup continues to work for both providers; each sees only its own migration set.

### Docker Compose profiles

The `postgres` service is declared under the `postgres` Compose profile so it only starts when explicitly requested:

```bash
# SQLite (default — no extra Docker service)
docker compose up

# PostgreSQL (opt-in)
DATABASE_PROVIDER=postgres docker compose --profile postgres up
```

## Consequences

### Positive

- Developers choose their engine once and use it consistently across all environments.
- SQLite remains the zero-friction default — clone, run, done.
- PostgreSQL is a first-class option for production-parity testing without requiring project-wide changes.
- The Compose profile approach prevents accidental PostgreSQL containers from starting in SQLite mode.
- `dotnet run` continues to work unchanged with SQLite.

### Negative

- Two parallel migration sets must be maintained. Adding a schema change requires migrations in both `Migrations/` and `Migrations/Npgsql/`.
- `ProviderSpecificMigrationsAssembly` uses an EF Core internal API (`MigrationsAssembly` from `Microsoft.EntityFrameworkCore.Migrations.Internal`), annotated with `#pragma warning disable EF1001`. This may require updates on major EF Core version upgrades.
- PostgreSQL support is not tested in CI (no Testcontainers integration yet — tracked in issue #353).

### Neutral

- The `DATABASE_URL` connection string format follows the Npgsql convention (`Host=...;Database=...;Username=...;Password=...`).
- `.env.example` documents all three new environment variables (`DATABASE_PROVIDER`, `DATABASE_URL`, `POSTGRES_PASSWORD`). `.env` is git-ignored.
