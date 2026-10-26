---
name: upgrade-runtime
description: Upgrade the .NET SDK / ASP.NET Core target framework or a major dependency version, updating every place the version is pinned (global.json, csproj, lock files, Dockerfile, CI/CD, docs). Use for .NET LTS upgrades or major package bumps.
---

# Upgrade the runtime

Dependencies, CI/CD, and Docker are on the "ask before changing" list: confirm
the target version with the user and make sure an issue exists first.

## Where the version lives

| File | What to change |
| ---- | -------------- |
| `global.json` | `sdk.version` |
| `src/Dotnet.Samples.AspNetCore.WebApi/*.csproj` | `<TargetFramework>`, every `PackageReference` version (check each package for a release that supports the new framework) |
| `test/Dotnet.Samples.AspNetCore.WebApi.Tests/*.csproj` | `<TargetFramework>`, test package versions |
| `packages.lock.json` (both projects) | regenerate with `dotnet restore --force-evaluate` |
| `Dockerfile` | `dotnet/sdk:<ver>` builder and `dotnet/aspnet:<ver>-alpine` runtime images |
| `.github/workflows/dotnet-ci.yml`, `dotnet-cd.yml` | `DOTNET_VERSION` env |
| `azure-pipelines.yml` | .NET version used by the pipeline, if pinned |
| `README.md`, `CLAUDE.md` | version mentions in the overview, badges, and Tech Stack |

## Steps

1. Update every file in the table; search for the old version string
   (e.g. `net9.0`, `9.0`) to catch anything missed.
2. Read the official breaking-changes list for the new version and fix
   compile errors and warnings.
3. Run `dotnet build --configuration Release` and
   `dotnet test --settings .runsettings`.
4. Run `docker compose build` and `docker compose up` to confirm the image
   starts and `GET /health` returns `200`.
5. If the upgrade changes an architectural decision, add an ADR (see the
   `create-adr` skill).
6. Run `/pre-commit`.
