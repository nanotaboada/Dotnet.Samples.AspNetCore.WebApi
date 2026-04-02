# Architecture Decision Records

This directory contains Architecture Decision Records (ADRs) for this project. ADRs document significant architectural decisions — what was decided, why, and what trade-offs were accepted.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [0001](0001-adopt-traditional-layered-architecture.md) | Adopt Traditional Layered Architecture | Accepted (Under Reconsideration) |
| [0002](0002-use-mvc-controllers-over-minimal-api.md) | Use MVC Controllers over Minimal API | Accepted |
| [0003](0003-use-sqlite-for-data-storage.md) | Use SQLite for Data Storage | Accepted |
| [0004](0004-use-uuid-as-database-primary-key.md) | Use UUID as Database Primary Key | Accepted |
| [0005](0005-use-squad-number-as-api-mutation-key.md) | Use Squad Number as API Mutation Key | Accepted |
| [0006](0006-use-rfc-7807-problem-details-for-errors.md) | Use RFC 7807 Problem Details for Errors | Accepted |
| [0007](0007-use-fluentvalidation-over-data-annotations.md) | Use FluentValidation over Data Annotations | Accepted |
| [0008](0008-use-automapper-for-dto-mapping.md) | Use AutoMapper for DTO Mapping | Accepted |
| [0009](0009-implement-in-memory-caching.md) | Implement In-Memory Caching | Accepted |
| [0010](0010-use-serilog-for-structured-logging.md) | Use Serilog for Structured Logging | Accepted |
| [0011](0011-use-docker-for-containerization.md) | Use Docker for Containerization | Accepted |
| [0012](0012-use-stadium-themed-semantic-versioning.md) | Use Stadium-Themed Semantic Versioning | Accepted |

## When to Create an ADR

Create an ADR when a decision affects:

- Project structure or overall architecture
- Technology choices or dependencies
- API contracts or data model design
- Non-functional requirements (performance, security, scalability)
- Development workflows or processes

## Template

```markdown
# [NUMBER]. [TITLE]

Date: YYYY-MM-DD

## Status

[Proposed | Accepted | Deprecated | Superseded by ADR-XXXX]

## Context

What is the issue we are facing? What forces are at play (technical, political,
social, project constraints)? Present facts neutrally without bias.

## Decision

We will [DECISION IN ACTIVE VOICE WITH FULL SENTENCES].

## Consequences

### Positive
- Consequence 1

### Negative
- Trade-off 1

### Neutral
- Other effect 1
```
