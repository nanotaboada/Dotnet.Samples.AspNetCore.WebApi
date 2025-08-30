# GitHub Copilot Instructions

These instructions guide GitHub Copilot on how to assist meaningfully within this repository.

## 🎯 Project Overview

This project is a proof-of-concept Web API built using:
- **.NET 8 (LTS)**
- **ASP.NET Core**
- **EF Core** with a **SQLite** database for simplicity
- **Docker Compose** for basic containerization

### Key Characteristics
- **Purpose**: Learning-focused PoC demonstrating modern ASP.NET Core patterns
- **Complexity**: Simple CRUD operations with a single `Player` entity
- **Focus**: Clean architecture, best practices, and maintainable code
- **Database**: SQLite for development; PostgreSQL may be introduced later

## ✅ Coding Conventions

Follow standard C# conventions:
- Use `PascalCase` for class names, methods, and public properties
- Use `camelCase` for local variables and private fields
- Use `async/await` consistently for asynchronous code
- Prefer `var` for local variable declarations where the type is obvious
- Nullable reference types are **enabled**
- Use `CSharpier` formatting standards (opinionated)

## 🏗️ Architectural Patterns

This project follows a **layered architecture** with clear separation of concerns:

### Layer Structure
- **Controllers** (`/Controllers`) - Handle HTTP requests and responses
- **Services** (`/Services`) - Contain business logic and orchestration
- **Repositories** (`/Repositories`) - Abstract data access layer
- **Models** (`/Models`) - Domain entities and DTOs
  - `Player` - Core domain entity
  - `PlayerRequestModel` - Input validation model
  - `PlayerResponseModel` - API response model
- **Validators** (`/Validators`) - FluentValidation rules
- **Mappings** (`/Mappings`) - AutoMapper configurations
- **Data** (`/Data`) - EF Core DbContext and database concerns

### Design Principles
- **Dependency Injection** for all services and repositories
- **Repository Pattern** for data access abstraction
- **Service Layer** for business logic encapsulation
- **AutoMapper** for clean object transformations
- **FluentValidation** for robust input validation
- **Async/Await** throughout the application stack

## ✅ Copilot Should Focus On

- Generating idiomatic ASP.NET Core controller actions
- Writing EF Core queries using LINQ
- Following async programming practices
- Producing unit tests using **xUnit**
- Suggesting dependency-injected services
- Adhering to RESTful naming and HTTP status codes
- Using `ILogger<T>` for logging
- Working with Docker-friendly patterns
- Implementing proper error handling and validation
- Using appropriate HTTP status codes (200, 201, 400, 404, 409, etc.)
- Following the existing caching patterns with `IMemoryCache`

## 🚫 Copilot Should Avoid

- Generating raw SQL unless explicitly required
- Using EF Core synchronous APIs (e.g., `FirstOrDefault` over `FirstOrDefaultAsync`)
- Suggesting static service or repository classes
- Including XML comments or doc stubs unless requested
- Suggesting patterns that conflict with DI (e.g., `new Service()` instead of constructor injection)
- Using `ConfigureAwait(false)` in ASP.NET Core contexts
- Implementing complex inheritance hierarchies when composition is simpler
- Adding unnecessary middleware or filters without clear purpose

## 🧪 Testing

- Use **xUnit**
- Use **Moq** for mocking
- Prefer testing **service logic** and **controller behavior**
- Place unit tests under `test/` following structure already present (e.g., `Unit/PlayerServiceTests.cs`)
- **Test Data**: Use faker patterns for consistent test data generation
- **Assertions**: FluentAssertions for readable test assertions

## ⚡ Performance & Best Practices

- Use `AsNoTracking()` for read-only EF Core queries
- Implement caching patterns with `IMemoryCache` for frequently accessed data
- Use `DbContextPool` for better performance (already configured)
- Follow async/await patterns consistently
- Validate input using **FluentValidation** before processing
- Use AutoMapper for object transformations
- Implement proper logging with structured logging patterns

## 🔧 Tooling & Environment

- Format code with **CSharpier**
- SQLite is used in development; **PostgreSQL** may be introduced in production later
- Code runs in a **Docker Compose** environment
- .NET 8 SDK is required
- All configurations live in `appsettings.*.json` files

## 🏷️ Technology Stack Deep Dive

### Entity Framework Core
- **DbContext**: `PlayerDbContext` with SQLite provider
- **Migrations**: Used for schema management and data seeding
- **Pooling**: `AddDbContextPool` for better performance
- **Query Optimization**: `AsNoTracking()` for read-only operations
- **Async Operations**: All database calls use async/await

### AutoMapper
- **Profile**: `PlayerMappingProfile` handles all object mappings
- **Bidirectional**: Maps between request/response models and entities
- **Integration**: Registered in DI container

### FluentValidation
- **Validators**: `PlayerRequestModelValidator` for input validation
- **Integration**: Automatic validation in controllers before processing
- **Error Messages**: Descriptive validation messages

### Caching Strategy
- **IMemoryCache**: Service-level caching for read operations
- **Cache Keys**: Consistent naming with `nameof()` pattern
- **Invalidation**: Cache cleared on data modifications
- **TTL**: Sliding expiration (10 min) + absolute expiration (1 hour)

### Logging with Serilog
- **Structured Logging**: Consistent log message templates
- **Log Levels**: Appropriate use of Information, Warning, Error
- **Context**: Include relevant data in log messages
- **Configuration**: File and console sinks configured

## 🧩 Folder Conventions

- `Controllers` for Web API endpoints
- `Services` for business logic
- `Repositories` for data access
- `Models` for domain and DTO objects
- `Mappings` for AutoMapper profiles
- `Validators` for FluentValidation rules
- `Utilities` for shared helper logic

## 🧘 General Philosophy

Keep things **simple, clear, and idiomatic**. This is a learning-focused PoC — clarity and maintainability win over overengineering.

## 📋 Common Patterns in This Codebase

### Repository Pattern
```csharp
// Generic repository base class
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet;
    // Standard CRUD operations with async/await
}

// Specific repository with custom queries
public class PlayerRepository : Repository<Player>, IPlayerRepository
{
    public async Task<Player?> FindBySquadNumberAsync(int squadNumber) =>
        await _dbSet.FirstOrDefaultAsync(p => p.SquadNumber == squadNumber);
}
```

### Service Layer Pattern
```csharp
public class PlayerService : IPlayerService
{
    // Dependencies injected via constructor
    // Business logic with caching
    // AutoMapper for transformations
    // Logging for observability
}
```

### Controller Pattern
```csharp
[ApiController]
[Route("players")]
public class PlayerController : ControllerBase
{
    // Minimal controllers - delegate to services
    // Proper HTTP status codes
    // FluentValidation integration
    // Structured logging
}
```

## 🎯 Domain Knowledge

### Player Entity Context
- **Squad Numbers**: Must be unique (1-99 typically)
- **Positions**: Football/Soccer positions with 2-character abbreviations (GK, RB, LB, CB, DM, CM, RW, AM, CF, SS, LW)
- **Starting11**: Boolean indicating if player is in starting lineup
- **Team/League**: String fields for team and league information

### Business Rules
- Squad numbers cannot be duplicated
- All operations should be logged for traceability
- Cache invalidation on data modifications
- Async operations throughout the stack

## 🚨 Error Handling Patterns

```csharp
// Controller level - return appropriate HTTP status codes
if (await playerService.RetrieveBySquadNumberAsync(squadNumber) != null)
{
    return TypedResults.Conflict($"Squad number {squadNumber} already exists");
}

// Service level - structured logging
logger.LogWarning("Player with squad number {SquadNumber} not found", squadNumber);

// Repository level - null handling
var player = await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
return player; // Let caller handle null
```

## 🔄 Data Flow Patterns

1. **Request Flow**: Controller → Validation → Service → Repository → Database
2. **Response Flow**: Database → Repository → Service → AutoMapper → Controller → Client
3. **Caching**: Service layer implements `IMemoryCache` for read operations
4. **Logging**: Structured logging at each layer for observability

## 🎯 When to Use Different Approaches

### Choose EF Core When:
- Simple CRUD operations (current use case)
- Rapid development needed
- Strong typing and compile-time checking preferred
- Schema migrations are important

### Consider Raw SQL/Dapper When:
- Complex queries with performance requirements
- Need fine-grained control over SQL
- Working with existing stored procedures
- Micro-service with minimal ORM overhead

## 🌐 API Design Guidelines

- Use proper HTTP verbs (GET, POST, PUT, DELETE)
- Return appropriate status codes (200, 201, 400, 404, 409, 500)
- Implement consistent error response formats
- Use route parameters for resource identification
- Apply validation before processing requests

## 🚀 Future Evolution Considerations

- **Database Migration**: SQLite → PostgreSQL transition path
- **Authentication**: JWT Bearer token implementation ready
- **API Versioning**: URL-based versioning strategy
- **OpenAPI**: Comprehensive Swagger documentation
- **Monitoring**: Health checks and metrics endpoints
- **Containerization**: Docker multi-stage builds optimized
