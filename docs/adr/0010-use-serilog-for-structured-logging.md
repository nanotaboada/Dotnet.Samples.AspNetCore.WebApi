# 0010. Use Serilog for Structured Logging

Date: 2026-04-02

## Status

Accepted

## Context

.NET provides a built-in logging abstraction (`Microsoft.Extensions.Logging`) with pluggable providers. Out of the box it supports console and debug output, but structured log output — where log events are emitted as queryable key-value pairs rather than plain strings — requires a third-party sink.

Structured logging is important even for a learning project because it demonstrates a production practice: log consumers (Elasticsearch, Seq, Datadog) ingest structured events and can filter, aggregate, and alert on specific fields. Emitting plain strings that embed values in a message template forfeits that capability.

Serilog is the most widely used structured logging library in the .NET ecosystem. It integrates with `ILogger<T>` via a host-level configuration, meaning application code never imports Serilog directly — it depends only on the framework abstraction.

## Decision

We will use Serilog, configured at host level in `Program.cs` via `UseSerilog()`. Two sinks are configured: console (for local development visibility) and file (for persistent log output under `logs/`). Message template parameters are passed as structured properties (`{@Player}`, `{SquadNumber}`), not interpolated strings, so that sinks that support structured output can index them as discrete fields.

## Consequences

### Positive
- Application code depends on `ILogger<T>` (the framework abstraction), not on Serilog directly. Swapping the logging backend requires only a `Program.cs` change.
- Structured log events support downstream querying and alerting in log management systems, demonstrating a production-grade logging practice.
- Serilog's enrichers (machine name, thread ID, environment) can be added declaratively in configuration without touching application code.
- The file sink provides a persistent log archive useful for debugging Docker container runs after the container exits.

### Negative
- Serilog is an additional dependency. Its host-level integration (`UseSerilog`) replaces the default ASP.NET Core logging pipeline, which can surprise contributors who expect the default provider behaviour.
- Misconfigured message templates (e.g., using string interpolation instead of structured parameters) silently degrade structured output without compile-time warnings.

### Neutral
- The project previously used `Microsoft.Extensions.Logging` directly (removed in PR #192). The migration to Serilog was driven by the need for file sink support and consistent structured output across the cross-language comparison set.
