# 0007. Use FluentValidation over Data Annotations

Date: 2026-04-02

## Status

Accepted

## Context

ASP.NET Core provides two primary mechanisms for validating request models: Data Annotations (attributes applied directly to model properties, e.g. `[Required]`, `[Range]`) and FluentValidation (a separate library that defines validation rules in dedicated validator classes).

Data Annotations are built into the framework and require no additional dependencies, but they are limited to declarative, property-level rules. Complex cross-property validation, async database checks (e.g., verifying that a squad number is unique), and operation-specific rules (different rules for create vs. update) are difficult or impossible to express with annotations alone.

The player creation flow requires an async uniqueness check against the database (`BeUniqueSquadNumber`). The update flow must skip that same check because the player already exists. This kind of operation-contextual validation is a first-class concern in FluentValidation via named rule sets.

## Decision

We will use FluentValidation for all request model validation. Validators are defined in dedicated classes under `Validators/` and registered with the DI container via `AddValidatorsFromAssemblyContaining<PlayerRequestModelValidator>()`. Validation rules are grouped into named rule sets (`Create` and `Update`) to make operation-specific behaviour explicit. Controllers invoke the appropriate rule set by name.

## Consequences

### Positive
- Validation logic lives in its own class, separate from the model and the controller, respecting the Single Responsibility Principle.
- Named rule sets (`Create`, `Update`) make it explicit which rules apply to which HTTP operations, eliminating hidden branching inside the validator.
- Async rules (e.g., `MustAsync(BeUniqueSquadNumber)`) integrate naturally, enabling database-backed validation without awkward workarounds.
- Validators are plain classes and trivial to unit test in isolation.

### Negative
- FluentValidation is an additional NuGet dependency not bundled with the framework.
- The `ValidateAsync` overload called internally is the `IValidationContext` overload, not the generic one — an implementation detail that must be respected when mocking validators in controller tests (see Copilot instructions for the correct Moq setup).
- Teams unfamiliar with FluentValidation face a learning curve around rule sets and the async API.

### Neutral
- FluentValidation does not replace model binding. Invalid JSON structure or type mismatches are still caught by the ASP.NET Core model binder before FluentValidation runs.
