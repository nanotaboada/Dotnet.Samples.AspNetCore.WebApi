# 0004. Use UUID as Database Primary Key

Date: 2026-04-02

## Status

Accepted

## Context

Every `Player` record requires a stable, unique identity for the database. The two common choices are a sequential integer (`int` / `IDENTITY`) and a UUID (`Guid`). Sequential integers are compact, human-readable, and index-friendly, but they leak information: an external caller who knows ID `42` can infer there are at least 42 records and probe adjacent IDs. UUIDs are opaque and decoupled from the database's row insertion order.

Crucially, the internal primary key is never surfaced in the API (see ADR 0005 for why `squadNumber` is the public-facing identifier). This means the choice of key type has no direct impact on API consumers.

## Decision

We will use `Guid` (`UUID v4`) as the primary key for the `Player` entity. The `Player` model declares `public Guid Id { get; set; } = Guid.NewGuid()`, which generates the value at the application layer before EF Core calls `SaveChanges`. `PlayerDbContext` additionally configures `entity.Property(player => player.Id).ValueGeneratedOnAdd()`, which signals to EF Core that the column is auto-generated — this is consistent with the field initializer approach and ensures migrations reflect the intended behaviour. The `Id` property is never included in API request or response models.

## Consequences

### Positive
- Opaque IDs prevent callers from guessing or enumerating records by probing sequential values.
- The field initializer (`Guid.NewGuid()`) generates the UUID at the application layer before the record reaches the database, decoupling identity generation from the persistence engine.
- Aligns with .NET defaults: `Guid.NewGuid()` requires no additional libraries or configuration.

### Negative
- `Guid` occupies 16 bytes vs 4 bytes for `int`, increasing index and storage size.
- Random UUID v4 values are not monotonically increasing, which can cause B-tree index fragmentation on write-heavy workloads. This is not a concern at this project's scale with SQLite, but would matter at production load on PostgreSQL or SQL Server.
- UUIDs are not human-readable, making raw database inspection or log correlation harder.

### Neutral
- Because `Id` is never exposed through the API, consumers are fully isolated from this decision. A future migration to sequential IDs or UUID v7 (time-ordered) would be a pure database/model change with no API contract impact.
