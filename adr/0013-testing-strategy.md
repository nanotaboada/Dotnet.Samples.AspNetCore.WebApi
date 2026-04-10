# 0013. Testing Strategy

Date: 2026-04-10

## Status

Accepted

## Context

The project is a learning reference for a REST API built with ASP.NET Core. Its primary goal is educational clarity — it must be immediately understandable to developers studying the stack, not just functional in production.

For a CRUD API of this size, a single suite of HTTP-layer integration tests (using `WebApplicationFactory<Program>` backed by an in-memory SQLite database) would cover the majority of meaningful risk. Such tests exercise the full request pipeline — routing, middleware, validation, serialization, and database interaction — with minimal setup.

The question was whether to stop there or to also add unit tests for each layer (controller, service, validator, repository) separately.

## Decision

The project implements the full test pyramid:

- **Unit tests** for each layer in isolation (controller, service, validator, repository), with dependencies replaced by Moq mocks or in-memory fakes.
- **Repository integration tests** that run EF Core queries against in-memory SQLite via the full migration chain, validating query correctness and migration health as a side effect.
- **HTTP integration tests** (`PlayerWebApplicationTests`) that send real HTTP requests through `WebApplicationFactory<Program>`, exercising the entire pipeline end-to-end.

Some overlap between layers is accepted deliberately.

## Rationale

The redundancy is the point. Having all test types in one place allows a reader to see exactly what each layer tests, what it isolates, and what it leaves to the layer above or below. The test suite is as much documentation as it is a safety net.

The repository integration tests in particular serve two purposes beyond what the HTTP tests provide: they validate individual EF Core queries in isolation (making failures easier to diagnose) and they run `MigrateAsync()` as a side effect, which acts as a canary for migration chain health.

## Consequences

### Positive

- Every layer of the architecture has dedicated tests, making failure diagnosis straightforward.
- The test suite demonstrates the full range of testing patterns available in the .NET ecosystem.
- Migration health is continuously validated as a side effect of the repository tests.

### Negative

- Test count is higher than strictly necessary for confidence.
- Maintenance cost is proportionally higher — changes to shared fixtures or fakes may require updates across multiple test classes.

### Neutral

- The HTTP integration tests (`PlayerWebApplicationTests`) are the minimum viable test suite. All other test classes add depth, not breadth.
- The `409 Conflict` branch on `POST /players` is unreachable via the HTTP pipeline: the `BeUniqueSquadNumber` rule in the `"Create"` validation rule set catches duplicates first and returns `400`. This path is covered by the controller unit test, where validation is mocked to pass.
