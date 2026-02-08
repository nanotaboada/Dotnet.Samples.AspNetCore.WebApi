# AGENTS.md

> **Token Efficiency**: Complete operational instructions (~2,550 tokens).
> **Auto-loaded**: NO (load explicitly with `#file:AGENTS.md` when needed)
> **When to load**: Complex workflows, troubleshooting, CI/CD setup, detailed architecture
> **Related files**: `#file:.github/copilot-instructions.md` (auto-loaded, ~650 tokens)

---

## Quick Start

```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run application
dotnet run --project src/Dotnet.Samples.AspNetCore.WebApi
# Server starts on https://localhost:9000

# View API documentation
# Open https://localhost:9000/swagger in browser

# View health check
curl https://localhost:9000/health
```

## .NET Version

This project targets **.NET 8 (LTS)**.

## Development Workflow

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage (using coverlet)
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report with settings from .runsettings
dotnet test --settings .runsettings

# Run specific test project
dotnet test test/Dotnet.Samples.AspNetCore.WebApi.Tests

# Run specific test method (filter by name)
dotnet test --filter "FullyQualifiedName~GetPlayers"

# Watch mode (auto-run tests on file changes)
dotnet watch test
```

**Coverage requirement**: Tests must maintain high coverage. Coverage reports are generated in `TestResults/` folder.

### Code Quality

```bash
# Build in Debug mode (default)
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Clean build artifacts
dotnet clean

# Format code (if dotnet-format is installed)
dotnet format

# Restore + Build + Test (full verification)
dotnet build && dotnet test
```

**Pre-commit checklist**:

1. Update CHANGELOG.md `[Unreleased]` section with your changes (Added/Changed/Fixed/Removed)
2. Run `dotnet build --configuration Release` - must build successfully
3. Run `dotnet test` - all tests must pass
4. Check code formatting and style
5. Follow conventional commit format (enforced by commitlint)

### Running the Application

```bash
# Run in development mode (hot reload enabled)
dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi

# Run in Release mode
dotnet run --project src/Dotnet.Samples.AspNetCore.WebApi --configuration Release

# Run published DLL directly
dotnet src/Dotnet.Samples.AspNetCore.WebApi/bin/Release/net8.0/Dotnet.Samples.AspNetCore.WebApi.dll
```

### Publishing

```bash
# Publish self-contained application (includes runtime)
dotnet publish -c Release -o publish

# Run published app
./publish/Dotnet.Samples.AspNetCore.WebApi

# Publish framework-dependent (requires .NET runtime on target)
dotnet publish -c Release --no-self-contained -o publish
```

### Database Management

```bash
# Database auto-initializes on first startup
# Pre-seeded database ships in src/Dotnet.Samples.AspNetCore.WebApi/Storage/players.db

# To reset database to seed state
rm src/Dotnet.Samples.AspNetCore.WebApi/Storage/players.db
# Next app startup will recreate via EF Core migrations + seeding

# Database location: src/Dotnet.Samples.AspNetCore.WebApi/Storage/players.db
```

**Important**: SQLite database with Entity Framework Core. Auto-migrates schema and seeds with football player data on first run.

## Docker Workflow

```bash
# Build container image
docker compose build

# Start application in container
docker compose up

# Start in detached mode (background)
docker compose up -d

# View logs
docker compose logs -f

# Stop application
docker compose down

# Stop and remove database volume (full reset)
docker compose down -v

# Health check (when running)
curl http://localhost:5000/health
```

**First run behavior**: Container initializes SQLite database with seed data. Volume persists data between runs.

## Release Management

### CHANGELOG Maintenance

**Important**: Update CHANGELOG.md continuously as you work, not just before releases.

**For every meaningful commit**:

1. Add your changes to the `[Unreleased]` section in CHANGELOG.md
2. Categorize under the appropriate heading:
   - **Added**: New features
   - **Changed**: Changes in existing functionality
   - **Deprecated**: Soon-to-be removed features
   - **Removed**: Removed features
   - **Fixed**: Bug fixes
   - **Security**: Security vulnerability fixes
3. Use clear, user-facing descriptions (not just commit messages)
4. Include PR/issue numbers when relevant (#123)

**Example**:

```markdown
## [Unreleased]

### Added
- User authentication with JWT tokens (#145)
- Rate limiting middleware for API endpoints

### Deprecated
- Legacy authentication endpoint /api/v1/auth (use /api/v2/auth instead)

### Fixed
- Null reference exception in player service (#147)

### Security
- Fix SQL injection vulnerability in search endpoint (#148)
```

### Creating a Release

When ready to release:

1. **Update CHANGELOG.md**: Move items from `[Unreleased]` to a new versioned section:

   ```markdown
   ## [1.1.0 - bernabeu] - 2026-02-15
   ```

2. **Commit and push** CHANGELOG changes
3. **Create and push tag**:

   ```bash
   git tag -a v1.1.0-bernabeu -m "Release 1.1.0 - Bernabéu"
   git push origin v1.1.0-bernabeu
   ```

4. **CD workflow runs automatically** to publish Docker images and create GitHub Release

See [CHANGELOG.md](CHANGELOG.md#how-to-release) for complete release instructions and stadium naming convention.

## CI/CD Pipeline

### Continuous Integration (dotnet.yml)

**Trigger**: Push to `main`/`master` or PR

**Jobs**:

1. **Setup**: .NET 8 SDK installation
2. **Restore**: `dotnet restore` with dependency caching
3. **Build**: `dotnet build --no-restore --configuration Release`
4. **Test**: `dotnet test --no-build --verbosity normal --settings .runsettings`
5. **Coverage**: Upload coverage reports to Codecov

**Local validation** (run this before pushing):

```bash
# Matches CI exactly
dotnet restore && \
dotnet build --no-restore --configuration Release && \
dotnet test --no-build --verbosity normal --settings .runsettings
```

### Azure Pipelines (azure-pipelines.yml)

Alternative CI/CD pipeline using Azure DevOps. Similar steps but with Azure-specific tasks.

## Project Architecture

**Structure**: Clean Architecture / Layered Architecture

```tree
src/Dotnet.Samples.AspNetCore.WebApi/
├── Program.cs                    # Application entry point, DI container setup
├── Controllers/                  # API endpoints
│   └── PlayersController.cs      # REST endpoints with Swagger annotations
├── Services/                     # Business logic
│   └── PlayersService.cs         # CRUD operations with caching
├── Data/                         # Database layer
│   ├── PlayersContext.cs         # EF Core DbContext
│   └── DbInitializer.cs          # Database seeding
├── Models/                       # Domain models & DTOs
│   ├── Player.cs                 # Entity model
│   └── PlayerDTO.cs              # Data Transfer Object
├── Validators/                   # FluentValidation rules
│   └── PlayerValidator.cs
├── Profiles/                     # AutoMapper mappings
│   └── PlayerProfile.cs
└── Storage/                      # SQLite database file

test/Dotnet.Samples.AspNetCore.WebApi.Tests/
├── ControllersTests/             # Controller unit tests
│   └── PlayersControllerTests.cs
└── ServicesTests/                # Service unit tests
    └── PlayersServiceTests.cs
```

**Key patterns**:

- ASP.NET Core 8.0 Minimal API style with controllers
- Entity Framework Core for database operations
- AutoMapper for DTO mappings
- FluentValidation for input validation
- In-memory caching with `IMemoryCache` (1-hour TTL)
- Serilog for structured logging (console + file)
- Swashbuckle for OpenAPI/Swagger documentation
- Dependency injection throughout
- Async/await for all I/O operations
- xUnit + Moq + FluentAssertions for testing

## Configuration

### appsettings.json

Key settings in `src/Dotnet.Samples.AspNetCore.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Storage/players.db"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  }
}
```

Override with environment variables:

```bash
export ConnectionStrings__DefaultConnection="Data Source=/custom/path/players.db"
dotnet run --project src/Dotnet.Samples.AspNetCore.WebApi
```

## API Endpoints

**Base URL**: `https://localhost:9000`

| Method | Path | Description |
| ------ | ---- | ----------- |
| `GET` | `/players` | Get all players (cached) |
| `GET` | `/players/{id}` | Get player by ID |
| `POST` | `/players` | Create new player |
| `PUT` | `/players/{id}` | Update player |
| `DELETE` | `/players/{id}` | Delete player |
| `GET` | `/health` | Health check |
| `GET` | `/swagger` | API documentation |

## Troubleshooting

### Port already in use

```bash
# Kill process on port 9000
lsof -ti:9000 | xargs kill -9
```

### Restore/build failures

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore with force
dotnet restore --force

# Clean and rebuild
dotnet clean && dotnet restore && dotnet build
```

### Database locked errors

```bash
# Stop all running instances
pkill -f "dotnet run"
pkill -f "Dotnet.Samples.AspNetCore.WebApi"

# Reset database
rm src/Dotnet.Samples.AspNetCore.WebApi/Storage/players.db
```

### EF Core migration issues

```bash
# This project uses EnsureCreated() for database initialization
# Schema is automatically created on first run

# To reset the database, delete the SQLite file:
rm src/Dotnet.Samples.AspNetCore.WebApi/Storage/players.db
# Database will be recreated on next startup
```

### SSL certificate issues (HTTPS)

```bash
# Trust development certificate
dotnet dev-certs https --trust

# Or use HTTP instead
dotnet run --project src/Dotnet.Samples.AspNetCore.WebApi --urls "http://localhost:9000"
```

### Test failures

```bash
# Run with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "FullyQualifiedName~PlayersControllerTests"
```

### Docker issues

```bash
# Clean slate
docker compose down -v
docker compose build --no-cache
docker compose up
```

## Testing the API

### Using Swagger UI (Recommended)

Open <https://localhost:9000/swagger> - Interactive documentation with "Try it out"

### Using curl

```bash
# Health check
curl https://localhost:9000/health -k

# Get all players
curl https://localhost:9000/players -k

# Get player by ID
curl https://localhost:9000/players/1 -k

# Create player
curl -X POST https://localhost:9000/players -k \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Pele",
    "lastName": "Nascimento",
    "club": "Santos",
    "nationality": "Brazil",
    "dateOfBirth": "1940-10-23",
    "squadNumber": 10
  }'

# Update player
curl -X PUT https://localhost:9000/players/1 -k
  -H "Content-Type: application/json"
  -d '{
    "firstName": "Diego",
    "lastName": "Maradona",
    "club": "Napoli",
    "nationality": "Argentina",
    "dateOfBirth": "1960-10-30",
    "squadNumber": 10
  }'

# Delete player
curl -X DELETE https://localhost:9000/players/1 -k
```

**Note**: `-k` flag skips SSL certificate verification for self-signed development certificates.

## Important Notes

- **CHANGELOG maintenance**: Update CHANGELOG.md `[Unreleased]` section with every meaningful change
- **Never commit secrets**: No API keys, tokens, or credentials in code
- **Test coverage**: Maintain high coverage with xUnit tests
- **Commit messages**: Follow conventional commits (enforced by commitlint in CI)
- **.NET version**: Must use .NET 8 (LTS) for consistency
- **Database**: SQLite is for demo/development only - not production-ready
- **HTTPS by default**: Development uses self-signed certificates (trust with `dotnet dev-certs https --trust`)
- **Caching**: In-memory cache clears on app restart
- **Logging**: Serilog writes to console and `logs/` folder
- **Rate limiting**: 100 requests per minute per IP (configurable in Program.cs)
- **AutoMapper**: DTO ↔ Entity mappings in Profiles/PlayerProfile.cs
- **FluentValidation**: Input validation rules in Validators/PlayerValidator.cs
- **Package references**: Use `dotnet add package` to maintain lock file
