# 0002. Use MVC Controllers over Minimal API

Date: 2026-04-02

## Status

Accepted

## Context

ASP.NET Core offers two approaches for building HTTP endpoints: traditional MVC controllers (attribute-routed classes inheriting from `ControllerBase`) and Minimal API (lambda-based route handlers registered directly in `Program.cs`). Microsoft has been investing in Minimal API as the preferred path for new greenfield services since .NET 6.

For this project, the primary concern is educational clarity. The codebase serves as a learning reference for developers exploring REST API patterns in .NET, and it is part of a cross-language comparison set where consistent structural conventions across stacks matter.

## Decision

We will use MVC controllers with attribute routing. Each controller is a class that encapsulates related HTTP handlers, receives its dependencies via constructor injection, and delegates all business logic to the service layer.

## Consequences

### Positive
- Controllers provide explicit, visible grouping of related endpoints in a single class.
- Constructor injection and interface-based dependencies make controllers straightforward to unit test with mocks.
- The pattern is familiar to developers coming from any MVC background (Spring MVC, Django CBVs, Rails controllers), lowering the barrier for the target audience.
- Attribute routing (`[HttpGet]`, `[HttpPost]`, etc.) keeps route definitions co-located with their handlers.

### Negative
- Controllers introduce more boilerplate than Minimal API: class declarations, action method signatures, and `[FromRoute]`/`[FromBody]` attributes.
- Minimal API is the direction Microsoft is actively investing in for new projects; staying on MVC controllers means diverging from that trajectory over time.
- The framework overhead of MVC (model binding pipeline, action filters, etc.) is larger than Minimal API, though negligible at this project's scale.

### Neutral
- This decision may be revisited if the project migrates to Clean Architecture (Issue #266), at which point Minimal API endpoints or vertical slice handlers could be a better fit for the new structure.
