---
name: database-schema-workflow
description: How to modify the Player entity schema, add EF Core migrations for both SQLite and PostgreSQL, and switch the active database provider (DATABASE_PROVIDER/DATABASE_URL/STORAGE_PATH). Use when changing entity fields, adding migrations, or configuring which database provider to run against.
---

**Modify schema**: Update `Player` entity → update DTOs → update AutoMapper profile → update `HasData()` seed data in `OnModelCreating` if needed → add migrations for both providers → update tests → run `dotnet test`.

```bash
# SQLite migration (default)
dotnet ef migrations add <Name> --project src/Dotnet.Samples.AspNetCore.WebApi
# PostgreSQL migration
DATABASE_PROVIDER=postgres DATABASE_URL="Host=localhost;..." \
  dotnet ef migrations add <Name> --project src/Dotnet.Samples.AspNetCore.WebApi --output-dir Migrations/Npgsql
```

**Switch database provider**:

- Set `DATABASE_PROVIDER=postgres` to use PostgreSQL, or leave unset for SQLite (default).
- `DATABASE_URL` (required for PostgreSQL) follows the Npgsql convention: `Host=...;Database=...;Username=...;Password=...`.
- For SQLite, `STORAGE_PATH` overrides the default file path (`AppContext.BaseDirectory/storage/players-sqlite3.db`).
- `ProviderSpecificMigrationsAssembly` filters migration discovery to the active provider's namespace at runtime — no code changes needed to switch.
- Migrations run automatically at startup via `MigrateAsync()`; no manual `dotnet ef database update` is required.
