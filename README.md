# 🧪 Web API made with .NET 8 (LTS) and ASP.NET Core

## Status

[![.NET CI](https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/actions/workflows/dotnet.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanotaboada_Dotnet.Samples.AspNetCore.WebApi&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nanotaboada_Dotnet.Samples.AspNetCore.WebApi)
[![Build Status](https://dev.azure.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/_apis/build/status%2FDotnet.Samples.AspNetCore.WebApi?branchName=master)](https://dev.azure.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/_build/latest?definitionId=14&branchName=master)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ac7b7e22f1cd4d9d9233b36982b0d6a9)](https://app.codacy.com/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![codecov](https://codecov.io/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/graph/badge.svg?token=hgJc1rStJ9)](https://codecov.io/gh/nanotaboada/Dotnet.Samples.AspNetCore.WebApi)
[![CodeFactor](https://www.codefactor.io/repository/github/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/badge)](https://www.codefactor.io/repository/github/nanotaboada/Dotnet.Samples.AspNetCore.WebApi)

## About

Proof of Concept for a Web API made with [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8) (LTS) and [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-8.0?view=aspnetcore-8.0).

## Structure

![Structure](/assets/images/Structure.svg)

_Figure: Simplified, conceptual project structure and main application flow. Not all dependencies are shown._

## Start

```console
dotnet watch run --project src/Dotnet.Samples.AspNetCore.WebApi/Dotnet.Samples.AspNetCore.WebApi.csproj
```

## Documentation (Development-only)

```console
https://localhost:9000/swagger/index.html
```

![API Documentation](/assets/images/Swagger.png)

## Container

### Docker Compose

This setup uses [Docker Compose](https://docs.docker.com/compose/) to build and run the app and manage a persistent SQLite database stored in a Docker volume.

#### Build the image

```bash
docker compose build
```

#### Start the app

```bash
docker compose up
```

> On first run, the container copies a pre-seeded SQLite database into a persistent volume
> On subsequent runs, that volume is reused and the data is preserved

#### Stop the app

```bash
docker compose down
```

#### Optional: database reset

```bash
docker compose down -v
```

> This removes the volume and will reinitialize the database from the built-in seed file the next time you `up`.

## Credits

The solution has been coded using [Visual Studio Code](https://code.visualstudio.com/) with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension.

## Terms

All trademarks, registered trademarks, service marks, product names, company names, or logos mentioned on this repository are the property of their respective owners. All usage of such terms herein is for identification purposes only and constitutes neither an endorsement nor a recommendation of those items. Furthermore, the use of such terms is intended to be for educational and informational purposes only.
