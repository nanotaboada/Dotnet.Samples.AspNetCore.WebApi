# 0003. Use SQLite for Data Storage

Date: 2026-04-02

## Status

Accepted

## Context

The project requires a relational database to persist football player data. The main candidates were SQLite, PostgreSQL, and SQL Server. The project is a proof-of-concept and learning reference, not a production service. Deployment simplicity and zero-configuration setup are higher priorities than scalability or advanced database features.

The cross-language comparison set (Go/Gin, Java/Spring Boot, Python/FastAPI, Rust/Rocket, TypeScript/Node.js) all use SQLite for the same reasons, so consistency across the set also favours it.

## Decision

We will use SQLite as the database engine, accessed through Entity Framework Core. The database file is stored at `storage/players-sqlite3.db` and is pre-seeded with sample data. Docker deployments mount the file into a named volume so data survives container restarts.

## Consequences

### Positive
- Zero-config: no server process, no connection string credentials, no Docker service dependency for local development.
- The database file can be committed to the repository as seed data, making onboarding instant.
- EF Core abstracts the SQL dialect, so migrating to another database requires changing only the provider registration.

### Negative
- SQLite does not support concurrent writes, making it unsuitable for multi-instance deployments or high-throughput scenarios.
- Some EF Core features (e.g., certain migration operations) behave differently with the SQLite provider.
- Not representative of a production database choice, which may mislead learners about real-world persistence decisions.

### Neutral
- Issue #249 tracks adding PostgreSQL support for environments that require a production-grade database. When implemented, SQLite will remain the default for local development and this ADR will be supplemented by a new ADR documenting the PostgreSQL decision.
