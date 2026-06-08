# CLAUDE.md

## Claude Code

- Run `/pre-commit` to execute the full pre-commit checklist for this project.

## Overview

REST API for managing football players built with ASP.NET Core 10. Implements CRUD operations with a layered architecture, EF Core persistence (SQLite by default, PostgreSQL opt-in via `DATABASE_PROVIDER`), FluentValidation, AutoMapper, and in-memory caching. Primarily a learning and reference project — clarity and educational value take precedence over brevity.

## Tech Stack

| Category        | Technology                                                |
|-----------------|-----------------------------------------------------------|
| Language        | C# (.NET 10 LTS)                                         |
| Framework       | ASP.NET Core (MVC controllers)                           |
| ORM             | Entity Framework Core 10                                  |
| Database        | SQLite (default) · PostgreSQL 17 (opt-in)                |
| Mapping         | AutoMapper                                                |
| Validation      | FluentValidation                                          |
| Caching         | `IMemoryCache` (10-min sliding + 1-hour absolute expiry) |
| Logging         | Serilog (structured, console + file)                     |
| Testing         | xUnit + Moq + FluentAssertions                           |
| Formatting      | CSharpier                                                 |
| Containerization| Docker                                                    |

## Structure

```tree
src/Dotnet.Samples.AspNetCore.WebApi/
├── Controllers/        — HTTP handlers; minimal logic, delegate to services        [HTTP layer]
├── Services/           — Business logic + IMemoryCache caching                     [business layer]
├── Repositories/       — Generic Repository<T> + specific implementations          [data layer]
├── Models/             — Player entity + request/response DTOs
├── Validators/         — FluentValidation validators (one per request model)
├── Mappings/           — AutoMapper profiles (PlayerMappingProfile)
├── Enums/              — Position abbreviations and other domain enumerations
├── Extensions/         — IServiceCollection extension methods (service registration)
├── Configurations/     — Options classes bound from appsettings.json
├── Middlewares/        — Custom ASP.NET Core middleware
├── Data/               — DbContext; seed data via HasData() in OnModelCreating
├── Migrations/         — EF Core migrations; Npgsql/ subdirectory for PostgreSQL provider
├── Utilities/          — Internal helpers: HttpContext, Swagger, PlayerData seed source
└── Storage/            — SQLite database file (runtime-generated, gitignored)

test/Dotnet.Samples.AspNetCore.WebApi.Tests/
├── Integration/        — Repository and WebApplication integration tests
├── Unit/               — Unit tests (controllers, services, validators)
└── Utilities/          — Shared test helpers: PlayerFakes, PlayerMocks, PlayerStubs
```

**Layer rule**: `Controller → Service → Repository → Database`. Controllers must not access repositories directly. Business logic must not live in controllers.

**Cross-cutting**: `Program.cs` wires health checks (`GET /health`), rate limiting, CORS (dev only), and Swagger UI (dev only). Serilog is configured at host level. All validators are registered via `AddValidatorsFromAssemblyContaining<PlayerRequestModelValidator>()`.

## Coding Guidelines

- **Naming**: PascalCase (public members), camelCase (private fields with `_` prefix)
- **DI**: Primary constructors everywhere
- **Async**: All I/O operations use `async`/`await`; no `ConfigureAwait(false)` (unnecessary in ASP.NET Core)
- **Reads**: Use `AsNoTracking()` for all EF Core read queries
- **Errors**: RFC 7807 Problem Details via `TypedResults.Problem(new HttpValidationProblemDetails(...))` for validation failures (422) and `TypedResults.Problem(statusCode: ...)` for other errors
- **Logging**: Structured logging via `ILogger<T>`; never `Console.Write`
- **Avoid**: synchronous EF Core APIs, controller business logic, static service/repository classes

### Test naming conventions

Two naming patterns, strictly by layer:

| Layer                | Location              | Pattern                                                       | Example                                                          |
|----------------------|-----------------------|---------------------------------------------------------------|------------------------------------------------------------------|
| Controller (unit)    | `test/.../Unit/`      | `{HttpMethod}_{Resource}_{Condition}_Returns{Outcome}`        | `Get_Players_Existing_ReturnsPlayers`                            |
| Service / Validator  | `test/.../Unit/`      | `{MethodName}_{StateUnderTest}_{ExpectedBehavior}`            | `RetrieveAsync_CacheMiss_QueriesRepositoryAndCachesResult`       |
| HTTP integration     | `test/.../Integration/` | `{HttpMethod}_{Resource}_{Condition}_Returns{Outcome}`      | `Get_Players_Existing_Returns200Ok`                              |

Each pattern has exactly three underscore-delimited segments where `{HttpMethod}_{Resource}` counts as the first segment. Do not add a fourth segment.

### FluentValidation rule sets

Validators use CRUD-named rule sets to make intent explicit. Use `RuleSet("Create", ...)` and `RuleSet("Update", ...)` — never anonymous / default rules.

```csharp
// "Create" rule set — POST /players
// Includes BeUniqueSquadNumber to prevent duplicate squad numbers on insert.
RuleSet("Create", () => {
    RuleFor(p => p.SquadNumber)
        .MustAsync(BeUniqueSquadNumber).WithMessage("SquadNumber must be unique.");
    // ... other rules
});

// "Update" rule set — PUT /players/squadNumber/{n}
// BeUniqueSquadNumber intentionally omitted: the player already exists in DB.
RuleSet("Update", () => {
    // ... same structural rules, no uniqueness check
});
```

Controllers must call the appropriate rule set explicitly:

```csharp
// POST
await validator.ValidateAsync(player, opts => opts.IncludeRuleSets("Create"));
// PUT
await validator.ValidateAsync(player, opts => opts.IncludeRuleSets("Update"));
```

### Mocking validators in controller tests

`ValidateAsync(T, Action<ValidationStrategy<T>>)` is a FluentValidation extension method. Internally it calls `ValidateAsync(IValidationContext, CancellationToken)`. Moq must target the **interface overload**, not the generic one:

```csharp
// ✅ Correct — matches the overload actually called at runtime
_validatorMock
    .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(new ValidationResult());

// ❌ Wrong — targets a different overload; mock is never hit → NullReferenceException
_validatorMock
    .Setup(v => v.ValidateAsync(It.IsAny<PlayerRequestModel>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(new ValidationResult());
```

Add `using FluentValidation;` to any test file that calls the rule set overload.

### Test utilities

`test/.../Utilities/` contains shared helpers used across all unit tests:

| Class           | Purpose                                                                 |
|-----------------|-------------------------------------------------------------------------|
| `PlayerFakes`   | Deterministic in-memory objects: `MakeNew()`, `MakeRequestModelForCreate()`, `MakeRequestModelForUpdate(n)`, `MakeFromStarting11(n)` |
| `PlayerMocks`   | Pre-configured `Mock<T>` setups for common scenarios                    |
| `PlayerStubs`   | Simple stub implementations where Moq would be overkill                 |

Always prefer `PlayerFakes` factory methods over constructing test data inline.

## Commands

### Quick Start

```bash
dotnet restore
dotnet build
dotnet run --project src/Dotnet.Samples.AspNetCore.WebApi           # https://localhost:9000
dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi     # hot reload
dotnet test --settings .runsettings                                 # with coverage
docker compose up
```

### Pre-commit Checks

1. Update `CHANGELOG.md` `[Unreleased]` section (Added / Changed / Fixed / Removed)
2. `dotnet build --configuration Release` — must succeed
3. `dotnet test --settings .runsettings` — all tests must pass
4. `dotnet csharpier .` — format; fix any reported issues
5. Commit message follows Conventional Commits format (enforced by commitlint)
6. If this commit introduces or changes an architectural decision, update CLAUDE.md and create or amend the relevant ADR in `docs/adr/`.

### Commits

Format: `type(scope): description (#issue)` — max 80 chars
Types: `feat` `fix` `chore` `docs` `test` `refactor` `ci` `perf`
Example: `feat(api): add player search endpoint (#123)`

## Agent Mode

### Proceed freely

- Route handlers and controllers
- Service layer logic and caching
- Repository implementations
- Unit and integration tests
- Documentation and CHANGELOG updates
- Bug fixes and refactoring within existing patterns

### Ask before changing

- Database schema (entity fields, migrations)
- Dependencies (`*.csproj`, `global.json`)
- CI/CD configuration (`.github/workflows/`)
- Docker setup
- Application configuration (`appsettings.json`)
- API contracts (breaking DTO changes)
- Caching strategy or TTL values
- FluentValidation rule set structure (adding or removing rule sets affects controller callers and tests)

### Never modify

- Production configurations or deployment secrets
- `.runsettings` coverage thresholds
- Port configuration (9000)
- Migration namespace constants in `ProviderSpecificMigrationsAssembly` — renaming breaks runtime provider filtering for one or both providers
- CD pipeline tag format (`vX.Y.Z-stadium`) or the stadium name sequence — names are assigned sequentially A→Z from the list in `CHANGELOG.md`; the next name is always the next unused letter

### Creating Issues

This project uses Spec-Driven Development (SDD): discuss in Plan mode first, create a GitHub Issue as the spec artifact, then implement. Always offer to draft an issue before writing code.

**Feature request** (`enhancement` label): Problem · Proposed Solution · Suggested Approach (optional) · Acceptance Criteria · References

**Bug report** (`bug` label): Description · Steps to Reproduce · Expected/Actual Behavior · Environment · Additional Context · Possible Solution (optional)

### Key workflows

**Add an endpoint**: Add DTO in `Models/` → update `PlayerMappingProfile` in `Mappings/` → add repository method(s) in `Repositories/` → add service method in `Services/` → add controller action in `Controllers/` → add/update validator rule set in `Validators/` → add tests in `test/.../Unit/` → run pre-commit checks.

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

## Invariants (never change without explicit discussion)

- **Port**: 9000 — configured via Docker (`Dockerfile` ENV and `compose.yaml` ports)
- **API contract**: endpoints, HTTP status codes, and response shapes are fixed; do not change them without explicit discussion
- **Commit format**: `type(scope): description (#issue)` — max 80 chars
- **Conventional Commits types**: `feat` `fix` `chore` `docs` `test` `refactor` `ci` `perf`
- **CHANGELOG.md** `[Unreleased]` section must be updated before every commit

## Architecture Decision Records

Significant architectural decisions are documented in `docs/adr/` (ADR-0001–0014).

Load `#file:docs/adr/README.md` when:
- The user asks about architectural choices or "why we use X"
- Proposing changes to core architecture or dependencies
- Historical context for past decisions is needed

Each ADR is self-contained. When a proposal would change an accepted decision, create a new ADR rather than editing the existing one.

**After completing work**: Suggest a branch name (e.g. `feat/add-player-search`) and a commit message following Conventional Commits including co-author line:

```text
feat(scope): description (#issue)

Co-authored-by: Claude Sonnet 4.6 <noreply@anthropic.com>
```
