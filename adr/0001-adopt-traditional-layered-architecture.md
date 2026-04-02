# 0001. Adopt Traditional Layered Architecture

Date: 2026-04-02

## Status

Accepted (Under Reconsideration — see [Issue #266](https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi/issues/266))

## Context

The project needed a structural pattern to organise the codebase for a REST API managing football player data. The primary goal is educational clarity — the project is a learning reference, so the pattern must be immediately understandable to developers familiar with standard layered architecture.

The two main candidates considered were traditional layered architecture (Controllers → Services → Repositories → Data) and Clean Architecture (with explicit domain, application, and infrastructure rings). Clean Architecture offers better separation of concerns and testability at the cost of significantly more boilerplate and conceptual overhead for a small PoC.

## Decision

We will organise the codebase into four layers: HTTP (`Controllers`, `Validators`), Business (`Services`, `Mappings`), Data (`Repositories`, `Data`), and a cross-cutting `Models` layer. Each layer depends only on the layer below it; controllers must not access repositories directly, and business logic must not live in controllers.

## Consequences

### Positive
- The pattern is widely understood and requires no prior knowledge of advanced architectural styles.
- The folder structure maps directly to responsibilities, making the codebase easy to navigate.
- Onboarding friction is low — contributors can locate any piece of logic by layer name alone.

### Negative
- Domain entities (`Player`) are shared across all layers, mixing persistence concerns with domain logic.
- Dependencies flow in multiple directions at the model level, limiting flexibility.
- Tight coupling between layers makes large-scale refactoring harder as the project grows.

### Neutral
- This decision is expected to be superseded when Issue #266 (Clean Architecture migration) is implemented. This ADR will remain in the repository as a record of the original decision and its context.
