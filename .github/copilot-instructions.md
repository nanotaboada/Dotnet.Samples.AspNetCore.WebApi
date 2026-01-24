# GitHub Copilot Instructions

> **⚡ Token Efficiency Note**: This is a minimal pointer file (~500 tokens, auto-loaded by Copilot).  
> For complete operational details, reference: `#file:AGENTS.md` (~2,500 tokens, loaded on-demand)  
> For specialized knowledge, use: `#file:SKILLS/<skill-name>/SKILL.md` (loaded on-demand when needed)

## 🎯 Quick Context

**Project**: ASP.NET Core 8 REST API demonstrating layered architecture patterns  
**Stack**: .NET 8 (LTS) • EF Core 9 • SQLite • Docker • xUnit  
**Pattern**: Repository + Service Layer + AutoMapper + FluentValidation  
**Philosophy**: Learning-focused PoC emphasizing clarity and best practices

## 📐 Core Conventions

- **Naming**: PascalCase (public), camelCase (private)
- **DI**: Primary constructors everywhere
- **Async**: All I/O operations use async/await
- **Logging**: Serilog with structured logging
- **Testing**: xUnit + Moq + FluentAssertions
- **Formatting**: CSharpier (opinionated)

## 🏗️ Architecture at a Glance

```
Controller → Service → Repository → Database
     ↓          ↓
Validation  Caching
```

- **Controllers**: Minimal logic, delegate to services
- **Services**: Business logic + caching with `IMemoryCache`
- **Repositories**: Generic `Repository<T>` + specific implementations
- **Models**: `Player` entity + Request/Response DTOs
- **Validators**: FluentValidation for input structure (business rules in services)

## ✅ Copilot Should

- Generate idiomatic ASP.NET Core code with minimal controller logic
- Use EF Core async APIs with `AsNoTracking()` for reads
- Follow Repository + Service pattern consistently
- Write tests with xUnit/Moq/FluentAssertions
- Apply RFC 7807 Problem Details for errors
- Use primary constructors for DI
- Implement structured logging with `ILogger<T>`

## 🚫 Copilot Should Avoid

- Synchronous EF Core APIs
- Controller business logic (belongs in services)
- Static service/repository classes
- `ConfigureAwait(false)` (unnecessary in ASP.NET Core)

## ⚡ Quick Commands

```bash
# Run with hot reload
dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi

# Test with coverage
dotnet test --settings .runsettings

# Docker
docker compose up

# Swagger: https://localhost:9000/swagger
```

## 📚 Need More Detail?

**For operational procedures**: Load `#file:AGENTS.md`  
**For Docker expertise**: Load `#file:SKILLS/docker-containerization/SKILL.md`  
**For testing patterns**: *(Planned)* `#file:SKILLS/testing-patterns/SKILL.md`

---

💡 **Why this structure?** Copilot auto-loads this file on every chat (~500 tokens). Loading `AGENTS.md` or `SKILLS/` explicitly gives you deep context only when needed, saving 80% of your token budget!
