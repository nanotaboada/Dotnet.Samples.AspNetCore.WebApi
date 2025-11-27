# 🧪 Web API made with .NET 8 (LTS) and ASP.NET Core

[![.NET CI](https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/actions/workflows/dotnet.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanotaboada_Dotnet.Samples.AspNetCore.WebApi&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nanotaboada_Dotnet.Samples.AspNetCore.WebApi)
[![Build Status](https://dev.azure.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/_apis/build/status%2FDotnet.Samples.AspNetCore.WebApi?branchName=master)](https://dev.azure.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/_build/latest?definitionId=14&branchName=master)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ac7b7e22f1cd4d9d9233b36982b0d6a9)](https://app.codacy.com/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![codecov](https://codecov.io/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/graph/badge.svg?token=hgJc1rStJ9)](https://codecov.io/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi)
[![CodeFactor](https://www.codefactor.io/repository/github/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/badge)](https://www.codefactor.io/repository/github/nanotaboada/Dotnet.Samples.AspNetCore.WebApi)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Table of Contents

- [About](#about)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Local Development](#local-development)
  - [Docker Deployment](#docker-deployment)
- [API Documentation](#api-documentation)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Contributing](#contributing)
- [License](#license)

## About

Proof of Concept for a Web API made with [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8) (LTS) and [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-8.0?view=aspnetcore-8.0). This project demonstrates modern ASP.NET Core patterns and best practices for building RESTful APIs.

## Features

- 🎯 **RESTful API** - Full CRUD operations for player management
- 🔄 **Async/Await** - Asynchronous operations throughout
- 🗄️ **Entity Framework Core** - SQLite database with code-first migrations
- 📊 **AutoMapper** - Clean object-to-object mapping
- ✅ **FluentValidation** - Robust input validation
- 💾 **Caching** - In-memory caching with IMemoryCache
- 📝 **Serilog** - Structured logging to console and file
- 🐳 **Docker** - Containerized deployment with Docker Compose
- 📖 **OpenAPI/Swagger** - Interactive API documentation
- 🏗️ **Layered Architecture** - Clear separation of concerns
- ⚡ **Rate Limiting** - Fixed window rate limiting
- 🏥 **Health Checks** - Built-in health endpoint

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional, for containerized deployment)
- [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) CLI tool (for database migrations)

  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Getting Started

### Local Development

1. Clone the repository

   ```bash
   git clone https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi.git
   cd Dotnet.Samples.AspNetCore.WebApi
   ```

2. Run the application

   ```bash
   dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi/Dotnet.Samples.AspNetCore.WebApi.csproj
   ```

3. Access the API
   - Swagger UI: `https://localhost:9000/swagger/index.html`
   - Health check: `https://localhost:9000/health`

### Docker Deployment

1. Build and start

   ```bash
   docker compose up --build
   ```

2. Stop (preserving data)

   ```bash
   docker compose down
   ```

3. Reset database

   ```bash
   docker compose down -v
   ```

> **Note:** On first run, the container copies a pre-seeded SQLite database into a persistent volume. Subsequent runs reuse that volume to preserve data.

## API Documentation

Interactive API documentation is available via Swagger UI in development mode at `https://localhost:9000/swagger/index.html`

![Swagger UI](/assets/images/Swagger.png)

**Available Endpoints:**

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/players` | Retrieve all players |
| `GET` | `/players/{id}` | Get player by ID (requires authentication) |
| `GET` | `/players/squad/{squadNumber}` | Get player by squad number |
| `POST` | `/players` | Create a new player |
| `PUT` | `/players/{squadNumber}` | Update player |
| `DELETE` | `/players/{squadNumber}` | Delete player |

## Technology Stack

| Category | Technology | Description |
|----------|------------|-------------|
| **Framework** | [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS) | Cross-platform runtime and SDK |
| **Web Framework** | [ASP.NET Core 8.0](https://learn.microsoft.com/aspnet/core) | High-performance web framework |
| **Database** | [SQLite](https://www.sqlite.org/) | Lightweight embedded database |
| **ORM** | [Entity Framework Core 9.0](https://learn.microsoft.com/ef/core) | Object-relational mapper |
| **Validation** | [FluentValidation](https://docs.fluentvalidation.net/) | Strongly-typed validation rules |
| **Mapping** | [AutoMapper](https://automapper.org/) | Object-to-object mapping |
| **Logging** | [Serilog](https://serilog.net/) | Structured logging library |
| **API Documentation** | [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) | Swagger/OpenAPI generator |
| **Testing** | [xUnit](https://xunit.net/), [Moq](https://github.com/devlooped/moq), [FluentAssertions](https://fluentassertions.com/) | Unit testing framework and libraries |
| **Containerization** | [Docker](https://www.docker.com/) & [Docker Compose](https://docs.docker.com/compose/) | Container platform and orchestration |

## Architecture

This project follows a **layered architecture** pattern with clear separation of concerns:

![Architecture Diagram](/assets/images/Structure.svg)

**Layers:**

- **Controllers** - HTTP request handling and routing
- **Services** - Business logic and orchestration
- **Repositories** - Data access abstraction
- **Data** - Entity Framework Core DbContext
- **Models** - Domain entities and DTOs

See [.github/copilot-instructions.md](.github/copilot-instructions.md) for detailed architectural patterns and conventions.

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on:

- Code conventions and commit message standards
- Pull request process
- Development workflow

Please also review our [Code of Conduct](CODE_OF_CONDUCT.md).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Disclaimer:** All trademarks and product names mentioned are the property of their respective owners. Their use here is for identification purposes only.
