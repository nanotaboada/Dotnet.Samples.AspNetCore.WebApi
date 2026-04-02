# 0012. Use Stadium-Themed Semantic Versioning

Date: 2026-04-02

## Status

Accepted

## Context

The project follows Semantic Versioning (`MAJOR.MINOR.PATCH`) for release numbers. Purely numeric version strings are accurate but forgettable — contributors and users rarely remember whether "2.1.0" came before or after "2.0.3" without consulting the changelog.

The project is football-themed (it manages football player data), and is part of a cross-language comparison set. A naming convention that reflects the project's domain makes releases memorable and reinforces the project's identity.

Several well-known projects use codename conventions: Ubuntu (alphabetical adjective-animal pairs), Android (alphabetical desserts until Android 10), macOS (California landmarks). The pattern is established and understood.

## Decision

We will append a football stadium codename to every release tag, following the format `vMAJOR.MINOR.PATCH-stadium` (e.g., `v2.1.0-dusseldorf`). Stadium names are drawn from a fixed, alphabetically ordered list of venues that hosted FIFA World Cup matches, documented in `CHANGELOG.md`. Names are assigned sequentially A→Z; the next release always uses the next unused letter. The tag format is enforced by the CD pipeline, which validates the stadium name before publishing.

## Consequences

### Positive
- Release names are memorable: "the dusseldorf release" is easier to recall and discuss than "2.1.0".
- Alphabetical ordering provides an implicit sequence — contributors can determine release order from the name alone without consulting version numbers.
- The convention is consistent and deterministic: there is no ambiguity about which name comes next.
- Reinforces the football domain theme throughout the project's lifecycle.

### Negative
- The convention is non-standard and may confuse first-time contributors who expect plain semantic version tags.
- The stadium list is finite (26 letters). If the project ever reaches 26 major releases, the list must be extended or the convention revisited.
- The CD pipeline validation adds a small amount of CI complexity to enforce the format.

### Neutral
- The stadium list is published in `CHANGELOG.md` under "Stadium Release Names" for reference. The current position in the sequence is always the last released tag — the next unused letter determines the next stadium name.
