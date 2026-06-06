# 0011. Use Docker for Containerization

Date: 2026-04-02

## Status

Accepted

## Context

The project needs to be runnable without requiring contributors to install the .NET SDK, configure environment variables, or manage database files manually. A containerised delivery format solves the "works on my machine" problem and demonstrates a standard deployment practice.

Docker is the de facto containerisation standard in the .NET ecosystem and across the cross-language comparison set that this project belongs to. Docker Compose provides a single-command local orchestration layer that handles image building, port mapping, and volume mounting without requiring knowledge of raw `docker run` flags.

## Decision

We will provide a multi-stage `Dockerfile` (build stage + runtime stage) and a `compose.yaml` for local orchestration. The application runs on port 9000 in the container, matching the local development port. A named Docker volume persists the SQLite database file across container restarts. The first run copies a pre-seeded database into the volume; subsequent runs reuse the existing volume.

## Consequences

### Positive
- `docker compose up` is the only command needed to run the full application from a fresh clone, with no SDK or database setup required.
- Multi-stage builds produce a minimal runtime image: only the published application artifacts are included, not the SDK or intermediate build outputs.
- The containerised environment closely mirrors what the CD pipeline builds and publishes to GitHub Container Registry, reducing environment-specific surprises.
- Port 9000 is consistent across all environments (local, Docker, CI), eliminating port-related configuration drift.

### Negative
- Docker Desktop is required on developer machines. On macOS and Windows, Docker Desktop is a large install with licensing implications for commercial use.
- The Docker abstraction layer adds a level of indirection that can make debugging harder for contributors unfamiliar with container networking or volume mounts.
- SQLite inside a Docker volume is not suitable for production use — it is an appropriate trade-off only because this project is explicitly a development and learning reference.

### Neutral
- Images are published to GitHub Container Registry (`ghcr.io`) as part of the CD pipeline, tagged by semantic version, stadium name, and `latest`. See the Releases section of `README.md` for the full tagging convention.
