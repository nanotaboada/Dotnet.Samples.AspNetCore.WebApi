# GitHub Copilot Instructions

## Overview

REST API for managing football players built with ASP.NET Core 10. Implements CRUD operations with a layered architecture, EF Core + SQLite persistence, FluentValidation, AutoMapper, and in-memory caching. Part of a cross-language comparison study (Go, Java, Python, Rust, TypeScript).

## Tech Stack

- **Language**: C# (.NET 10 LTS)
- **Framework**: ASP.NET Core (MVC controllers)
- **ORM**: Entity Framework Core 10
- **Database**: SQLite
- **Mapping**: AutoMapper
- **Validation**: FluentValidation
- **Caching**: `IMemoryCache` (1-hour TTL)
- **Logging**: Serilog (structured, console + file)
- **Testing**: xUnit + Moq + FluentAssertions
- **Formatting**: CSharpier
- **Containerization**: Docker

## Structure

```text
src/Dotnet.Samples.AspNetCore.WebApi/
├── Controllers/        — HTTP handlers; minimal logic, delegate to services        [HTTP layer]
├── Services/           — Business logic + IMemoryCache caching                     [business layer]
├── Repositories/       — Generic Repository<T> + specific implementations          [data layer]
├── Models/             — Player entity + DTOs
├── Validators/         — FluentValidation (structure only; business rules in services)
├── Profiles/           — AutoMapper profiles
├── Data/               — DbContext + DbInitializer
└── Storage/            — SQLite database file

test/Dotnet.Samples.AspNetCore.WebApi.Tests/
├── ControllersTests/
└── ServicesTests/
```

**Layer rule**: `Controller → Service → Repository → Database`. Controllers must not access repositories directly. Business logic must not live in controllers.

## Coding Guidelines

- **Naming**: PascalCase (public members), camelCase (private fields)
- **DI**: Primary constructors everywhere
- **Async**: All I/O operations use `async`/`await`; no `ConfigureAwait(false)` (unnecessary in ASP.NET Core)
- **Reads**: Use `AsNoTracking()` for all EF Core read queries
- **Errors**: RFC 7807 Problem Details for all error responses
- **Logging**: Structured logging via `ILogger<T>`; never `Console.Write`
- **Tests**: xUnit + Moq + FluentAssertions; test naming mirrors method under test
- **Avoid**: synchronous EF Core APIs, controller business logic, static service/repository classes

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
4. Verify code formatting with CSharpier
5. Commit message follows Conventional Commits format (enforced by commitlint)

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
- CI/CD configuration (`.github/workflows/`, `azure-pipelines.yml`)
- Docker setup
- Application configuration (`appsettings.json`)
- API contracts (breaking DTO changes)
- Caching strategy or TTL values

### Never modify

- Production configurations or deployment secrets
- `.runsettings` coverage thresholds
- Port configuration (9000)
- Database type (SQLite — demo/dev only)

### Key workflows

**Add an endpoint**: Add DTO in `Models/` → add service method in `Services/` → add controller action in `Controllers/` → add validator in `Validators/` → add tests → run pre-commit checks.

**Modify schema**: Update `Player` entity → update DTOs → update AutoMapper profile → reset `Storage/players.db` → update tests → run `dotnet test`.

**After completing work**: Suggest a branch name (e.g. `feat/add-player-search`) and a commit message following Conventional Commits including co-author line:

```text
feat(scope): description (#issue)

Co-authored-by: Copilot <175728472+Copilot@users.noreply.github.com>
```
