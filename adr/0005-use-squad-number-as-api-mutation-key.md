# 0005. Use Squad Number as API Mutation Key

Date: 2026-04-02

## Status

Accepted

## Context

The API exposes endpoints for retrieving, updating, and deleting individual players. Each of these operations requires a route parameter that identifies the target player. Two candidates exist: the internal UUID primary key (`Id`) and the domain-meaningful squad number (`SquadNumber`).

Exposing the UUID would couple API consumers to an internal database concern. UUIDs carry no domain meaning — a caller who knows that player "Lionel Messi" wears number 10 cannot construct the URL from that knowledge alone; they would first need to discover the UUID from a prior `GET /players` call.

`SquadNumber` is the natural identifier for a football player within a team context. It is the value coaches, fans, and data consumers use to refer to players. It is also the field that other API operations (`POST` uniqueness check, `PUT` route matching) already reason about.

## Decision

We will use `squadNumber` as the route parameter for mutation endpoints and primary lookup routes: `GET /players/squadNumber/{n}`, `PUT /players/squadNumber/{n}`, and `DELETE /players/squadNumber/{n}`. The internal `Id` (UUID) is never included in any request model or response model. A secondary `GET /players/{id:Guid}` route exists for internal or administrative lookups by UUID but is not part of the primary API contract.

## Consequences

### Positive
- URLs are human-readable and predictable from domain knowledge alone (`/players/squadNumber/10` unambiguously refers to the number-10 player).
- API consumers are fully decoupled from the internal persistence model.
- Consistency: the same identifier used in the domain (`SquadNumber`) is used in the API surface, reducing the mental translation between domain concepts and HTTP calls.

### Negative
- Squad numbers are unique only within a single team and only for the duration of a season. They can be reassigned when a player leaves or retires. This project models a single team's squad, so cross-team ambiguity is not a concern, but the limitation is real.
- Uniqueness must be enforced at the application layer. The `BeUniqueSquadNumber` async validator rule in the `Create` rule set guards against duplicate squad numbers on insert; the `Update` rule set intentionally omits this check because the player already exists.
- `squadNumber` is not a stable long-term identifier — if squad number reassignment were ever supported, consumers holding a bookmarked URL would silently reach a different player.

### Neutral
- The `GET /players/{id:Guid}` endpoint exists as a secondary route for internal or administrative lookups by UUID. It is intentionally excluded from the primary API contract and is not the recommended path for general consumers.
