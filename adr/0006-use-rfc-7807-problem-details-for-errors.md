# 0006. Use RFC 7807 Problem Details for Errors

Date: 2026-04-02

## Status

Accepted

## Context

HTTP APIs need a consistent format for communicating errors to consumers. Without a standard, each endpoint might return a different error shape — plain strings, custom JSON objects, or raw status codes with empty bodies — making client-side error handling fragile and unpredictable.

RFC 7807 (Problem Details for HTTP APIs) defines a standard JSON structure for error responses with well-known fields (`type`, `title`, `status`, `detail`, `instance`). ASP.NET Core 7+ includes built-in support for this format via `TypedResults.Problem` and `TypedResults.ValidationProblem`.

## Decision

We will use RFC 7807 Problem Details for all error responses. Validation failures will use `TypedResults.ValidationProblem`, which extends the standard shape with a `errors` field containing field-level messages. All other errors (not found, conflict, internal server error) will use `TypedResults.Problem` with an appropriate HTTP status code and a human-readable `detail` field.

## Consequences

### Positive
- A single, predictable error shape across all endpoints simplifies client-side error handling — consumers check `status` and `detail` without branching on response format.
- The format is an IETF standard, meaning it is recognisable to developers and interoperable with API tooling, gateways, and monitoring systems that understand Problem Details.
- Built-in ASP.NET Core support means no custom serialisation code is needed.
- Validation errors include field-level detail via the `errors` dictionary, giving consumers enough context to display meaningful feedback.

### Negative
- The response payload is more verbose than a plain string message, which adds minor overhead for simple error cases.
- Consumers must understand the Problem Details schema to interpret `type` URIs and distinguish error categories beyond HTTP status codes.

### Neutral
- The `type` field conventionally holds a URI that identifies the problem type. Most responses rely on the ASP.NET Core default, but individual controllers may override it with problem-specific URIs where appropriate — for example, `PlayerController` sets `Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409"` for conflict responses. In a production API, all `type` URIs would point to project-owned documentation pages.
