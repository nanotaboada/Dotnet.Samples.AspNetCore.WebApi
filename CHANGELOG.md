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

### Fixed

### Removed

---

## How to Release

To create a new release:

1. Update this CHANGELOG with release notes under the appropriate version heading
2. Create and push a version tag with stadium name:

   ```bash
   git tag -a v1.0.0-azteca -m "Release 1.0.0 - Azteca"
   git push origin v1.0.0-azteca
   ```

3. The CD workflow will automatically:
   - Build and test the project
   - Publish Docker images to GHCR with three tags (`:1.0.0`, `:azteca`, `:latest`)
   - Create a GitHub Release with auto-generated notes

---

<!-- Template for new releases:

## [X.Y.Z - STADIUM_NAME] - YYYY-MM-DD

### Added
- New features

### Changed
- Changes in existing functionality

### Fixed
- Bug fixes

### Removed
- Removed features

-->
