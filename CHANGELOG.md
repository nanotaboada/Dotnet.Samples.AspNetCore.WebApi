# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Stadium Release Names

This project uses famous football stadiums (A-Z) that hosted FIFA World Cup matches (with notable fallbacks where necessary):

| Letter | Stadium Name | Location | Tag Name |
| ------ | ----------- | -------- | -------- |
| A | Azteca | Mexico (1970, 1986, 2026) | `azteca` |
| B | Bernabéu | Spain (1982) | `bernabeu` |
| C | Centenario | Uruguay (1930) | `centenario` |
| D | Düsseldorf (Merkur Spiel-Arena) | Germany (2006) | `dusseldorf` |
| E | Ekaterinburg Arena | Russia (2018) | `ekaterinburg` |
| F | Frankfurt Waldstadion | Germany (1974, 2006) | `frankfurt` |
| G | Gelsenkirchen | Germany (2006) | `gelsenkirchen` |
| H | Hard Rock Stadium | USA (2026) | `hardrock` |
| I | Ibn Batouta Stadium | Morocco (2030) | `ibnbatouta` |
| J | Johannesburg Soccer City | South Africa (2010) | `johannesburg` |
| K | Kazan Arena | Russia (2018) | `kazan` |
| L | Lusail | Qatar (2022) | `lusail` |
| M | Maracanã | Brazil (1950, 2014) | `maracana` |
| N | Nantes Beaujoire | France (1998) | `nantes` |
| O | Olympiastadion Berlin | Germany (1974, 2006) | `olympiastadion` |
| P | Parc des Princes | France (1938, 1998) | `parcdesprinces` |
| Q | Qatar 974 | Qatar (2022) | `qatar974` |
| R | Rose Bowl | USA (1994) | `rosebowl` |
| S | San Siro | Italy (1934, 1990) | `sansiro` |
| T | Toronto BMO Field | Canada (2026) | `toronto` |
| U | Ullevi | Sweden (1958) | `ullevi` |
| V | Volgograd Arena | Russia (2018) | `volgograd` |
| W | Wembley | England (1966) | `wembley` |
| X | Xiamen Egret Stadium | (famous fallback) | `xiamen` |
| Y | Yokohama International Stadium | Japan (2002) | `yokohama` |
| Z | Zentralstadion Leipzig | Germany (1974, 2006) | `zentralstadion` |

---

## [Unreleased]

### Added

### Changed

### Deprecated

### Removed

### Fixed

### Security

---

## [1.1.0 - bernabeu] - 2026-02-09

### Changed

- Upgrade to .NET 10 LTS from .NET 8 (#368)
- Update Microsoft.AspNetCore.OpenApi to 10.0.0
- Update Microsoft.EntityFrameworkCore.Sqlite to 10.0.0
- Update Microsoft.EntityFrameworkCore.Design to 10.0.0
- Update Microsoft.VisualStudio.Web.CodeGeneration.Design to 10.0.0
- Update Docker images to .NET 10 SDK and runtime (now based on Ubuntu 24.04 LTS instead of Debian 12)
- Update Dockerfile user creation commands for Ubuntu compatibility (`groupadd`/`useradd` instead of `adduser`)
- Update CI/CD pipelines to use .NET 10 SDK
- Token efficiency strategy for Copilot/AI agents with optimized instruction loading and improved token counting script (#364)
- Bump Swashbuckle.AspNetCore from 10.1.0 to 10.1.2
- Bump docker/login-action from 3.6.0 to 3.7.0
- Bump softprops/action-gh-release from 2.2.0 to 2.5.0
- Bump actions/checkout from 6.0.1 to 6.0.2

---

## [1.0.0 - azteca] - 2026-01-22

Initial release. See [README.md](README.md) for complete feature list and documentation.

---

## How to Release

To create a new release, follow these steps in order:

### 1. Update CHANGELOG.md

Move items from the `[Unreleased]` section to a new release section:

```markdown
## [X.Y.Z - STADIUM_NAME] - YYYY-MM-DD

### Added
- New features here

### Changed
- Changes here

### Fixed
- Bug fixes here

### Removed
- Removed features here
```

**Important:** Commit and push this change before creating the tag.

### 2. Create and Push Version Tag

```bash
git tag -a vX.Y.Z-stadium -m "Release X.Y.Z - Stadium"
git push origin vX.Y.Z-stadium
```

Example:

```bash
git tag -a v1.0.0-azteca -m "Release 1.0.0 - Azteca"
git push origin v1.0.0-azteca
```

### 3. Automated CD Workflow

The CD workflow automatically:

- ✅ Validates the stadium name against the A-Z list
- ✅ Builds and tests the project in Release configuration
- ✅ Publishes Docker images to GHCR with three tags (`:X.Y.Z`, `:stadium`, `:latest`)
- ✅ Creates a GitHub Release with auto-generated notes from commits

### Pre-Release Checklist

- [ ] CHANGELOG.md updated with release notes
- [ ] CHANGELOG.md changes committed and pushed
- [ ] Tag created with correct format: `vX.Y.Z-stadium`
- [ ] Stadium name is valid (A-Z from table above)
- [ ] Tag pushed to trigger CD workflow

---

<!-- Template for new releases:

## [X.Y.Z - STADIUM_NAME] - YYYY-MM-DD

### Added
- New features

### Changed
- Changes in existing functionality

### Deprecated
- Soon-to-be removed features

### Removed
- Removed features

### Fixed
- Bug fixes

### Security
- Security vulnerability fixes

-->

---

[unreleased]: https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/compare/v1.1.0-bernabeu...HEAD
[1.1.0 - bernabeu]: https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/compare/v1.0.0-azteca...v1.1.0-bernabeu
[1.0.0 - azteca]: https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/releases/tag/v1.0.0-azteca
