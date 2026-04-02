# 0009. Implement In-Memory Caching

Date: 2026-04-02

## Status

Accepted

## Context

The player list endpoint (`GET /players`) queries the database on every request. For a read-heavy API where the data changes infrequently, this is unnecessary load. Caching the result reduces database round-trips and demonstrates a common performance pattern for learners.

The two main caching approaches in .NET are `IMemoryCache` (process-local, in-memory) and distributed caching (Redis, SQL Server — shared across multiple instances via `IDistributedCache`). Distributed caching is appropriate for multi-instance deployments where cache consistency across nodes matters.

This project runs as a single instance (local development or a single Docker container) with no horizontal scaling requirement.

## Decision

We will use `IMemoryCache` managed in the service layer (`PlayerService`). Cache entries use a 10-minute sliding expiration combined with a one-hour absolute expiration: each access within the sliding window resets the TTL, but the entry is always evicted after one hour regardless of access frequency. On a cache miss, the service queries the repository, stores the result under a well-known key with these options, and returns it. Write operations (create, update, delete) invalidate the cache entry so the next read reflects the current state.

## Consequences

### Positive
- Zero additional infrastructure: `IMemoryCache` is built into `Microsoft.Extensions.Caching.Memory` with no external service dependency.
- The service layer caching pattern is easy to follow and demonstrates the cache-aside pattern clearly for learners.
- Reduces database queries for the most frequent read operation without any changes to the repository or controller layers.

### Negative
- Cache state is lost on every application restart, so the first request after a restart always hits the database.
- `IMemoryCache` is not shared across multiple instances. If this application were scaled horizontally, each instance would maintain its own independent cache, potentially serving stale or inconsistent data across nodes.
- Memory usage grows with the size of the cached data; unbounded caching of large datasets in a long-running process could contribute to memory pressure.

### Neutral
- If the project ever moves to a multi-instance deployment (e.g., Kubernetes), replacing `IMemoryCache` with a Redis-backed `IDistributedCache` would require changes only in the service layer and `Program.cs` registration — the repository and controller layers are unaffected.
