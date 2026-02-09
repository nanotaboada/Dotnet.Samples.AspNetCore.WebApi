# GitHub Copilot Instructions

> **Token Budget**: Target 600, limit 650 (auto-loaded)
> Details: `#file:AGENTS.md` (~2,550 tokens, on-demand)
> Skills: `#file:SKILLS/<name>/SKILL.md` (on-demand)

## Quick Context

ASP.NET Core 10 REST API with layered architecture
**Stack**: .NET 10 LTS, EF Core 10, SQLite, Docker, xUnit
**Pattern**: Repository + Service + AutoMapper + FluentValidation
**Focus**: Learning PoC emphasizing clarity and best practices

## Core Conventions

- **Naming**: PascalCase (public), camelCase (private)
- **DI**: Primary constructors everywhere
- **Async**: All I/O operations use async/await
- **Logging**: Serilog with structured logging
- **Testing**: xUnit + Moq + FluentAssertions
- **Formatting**: CSharpier
- **Commits**: Subject ≤80 chars, include issue number (#123), body lines ≤80 chars, conventional commits

## Architecture

```text
Controller → Service → Repository → Database
     ↓          ↓
Validation  Caching
```

Controllers: Minimal logic, delegate to services
Services: Business logic + `IMemoryCache` caching
Repositories: Generic `Repository<T>` + specific implementations
Models: `Player` entity + DTOs
Validators: FluentValidation (structure only, business rules in services)

## Copilot Should

- Generate idiomatic ASP.NET Core code with minimal controller logic
- Use EF Core async APIs with `AsNoTracking()` for reads
- Follow Repository + Service pattern consistently
- Write tests with xUnit/Moq/FluentAssertions
- Apply RFC 7807 Problem Details for errors
- Use primary constructors for DI
- Implement structured logging with `ILogger<T>`

## Copilot Should Avoid

- Synchronous EF Core APIs
- Controller business logic (belongs in services)
- Static service/repository classes
- `ConfigureAwait(false)` (unnecessary in ASP.NET Core)

## Quick Commands

```bash
# Run with hot reload
dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi

# Test with coverage
dotnet test --settings .runsettings

# Docker
docker compose up

# Swagger: https://localhost:9000/swagger
```

## Load On-Demand Files

**Load `#file:AGENTS.md` when:**
- "How do I run tests with coverage?"
- "CI/CD pipeline setup or troubleshooting"
- "Database migration procedures"
- "Publishing/deployment workflows"
- "Detailed troubleshooting guides"

**Load `#file:SKILLS/<skill-name>/SKILL.md` (planned):**
- Docker optimization: `docker-containerization/SKILL.md`
- Testing patterns: `testing-patterns/SKILL.md`

**Human-readable overview**: See `README.md` (not auto-loaded)

---

**Why this structure?** Base instructions (~600 tokens) load automatically. On-demand files (~2,550 tokens) load only when needed, saving 80% of tokens per chat.
