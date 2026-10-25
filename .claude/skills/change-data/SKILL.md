---
name: change-data
description: Change the Player entity schema or the seed dataset, add EF Core migrations for both SQLite and PostgreSQL, keep test fakes in sync, and switch the active database provider (DATABASE_PROVIDER/DATABASE_URL/STORAGE_PATH). Use when changing entity fields, seed players, or migrations.
---

# Change data

Schema changes are on the "ask before changing" list: confirm with the user and
make sure an issue exists first. Never rename the migration namespace constants
in `Migrations/ProviderSpecificMigrationsAssembly.cs`.

All paths below are relative to `src/Dotnet.Samples.AspNetCore.WebApi/` unless
they start with `test/`.

## Schema change

1. Update the `Player` entity in `Models/Player.cs`, then the DTOs and
   `Mappings/PlayerMappingProfile.cs`.
2. Update the entity configuration in `Data/PlayerDbContext.cs` if needed.
3. Add one migration per provider (same name for both), running from the
   repository root:

   ```bash
   # SQLite (default)
   dotnet ef migrations add <Name> --project src/Dotnet.Samples.AspNetCore.WebApi
   # PostgreSQL
   DATABASE_PROVIDER=postgres DATABASE_URL="Host=localhost;..." \
     dotnet ef migrations add <Name> --project src/Dotnet.Samples.AspNetCore.WebApi --output-dir Migrations/Npgsql
   ```

4. Review both generated migrations; do not edit migrations that are already
   merged.

## Seed data change

1. Update `Utilities/PlayerData.cs` — it is the single source of truth for
   `HasData()` in `PlayerDbContext` and for `PlayerFakes` in tests.
2. Add a data migration for both providers (as above) so existing databases
   get the change; `MigrateAsync()` applies it at startup.
3. Check `test/.../Utilities/PlayerFakes.cs` and any test that asserts on
   specific players (squad numbers, counts).

## Switch database provider

- `DATABASE_PROVIDER=postgres` selects PostgreSQL; unset (or `sqlite`) keeps
  SQLite.
- `DATABASE_URL` (required for PostgreSQL) uses the Npgsql format:
  `Host=...;Database=...;Username=...;Password=...`.
- `STORAGE_PATH` overrides the SQLite file path (default
  `AppContext.BaseDirectory/storage/players-sqlite3.db`).
- `ProviderSpecificMigrationsAssembly` loads only the active provider's
  migrations; no code change is needed to switch.

## Finish

Run `dotnet test --settings .runsettings`, update the Database section of
`README.md` if behavior changed, then run `/pre-commit`.
