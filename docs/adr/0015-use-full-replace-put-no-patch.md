# 0015. Use Full-Replace PUT as the Partial Update Strategy

Date: 2026-06-10

## Status

Accepted

## Context

HTTP defines two methods for updating an existing resource:

- **PUT** — full replacement; the client sends the complete resource
  representation, and the server replaces the stored state entirely
- **PATCH** — partial update; the client sends only the changed fields

Both are standard and well-understood. The choice affects API surface
complexity, client implementation requirements, and server-side
validation logic.

## Decision

We use PUT for all player update operations
(`PUT /players/squadNumber/{n}`). The request body must contain the
full player representation; the server replaces the stored resource
entirely. No PATCH endpoint is provided at this time. A PATCH
implementation is tracked in the project backlog and remains under
active consideration.

## Consequences

### Positive
- Simpler server-side implementation: a single validation path,
  no partial-update merge logic
- PUT semantics are idempotent and well-understood by API consumers
- Consistent with all sibling repos in the cross-language comparison set

### Negative
- Clients must send the full resource representation even for
  single-field changes
- Fine-grained partial updates require a GET followed by a full PUT
- PATCH tracked in backlog — if implemented, this ADR will be superseded

### Neutral
- Standard REST semantics; no ambiguity about the update contract
  for current consumers
