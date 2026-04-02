# 0008. Use AutoMapper for DTO Mapping

Date: 2026-04-02

## Status

Accepted

## Context

The API separates its public contract (request/response DTOs) from its internal persistence model (`Player` entity). Every controller action that reads or writes data must map between these representations. Without a mapping library, this means writing explicit property-assignment code (`responseModel.Name = entity.Name`, etc.) in every service method — boilerplate that grows linearly with the number of fields and operations.

AutoMapper provides convention-based mapping: if property names match between source and destination, the mapping is configured once in a profile and applied everywhere. Custom mappings (renaming, transforming, ignoring fields) are expressed declaratively in the profile class.

## Decision

We will use AutoMapper for all DTO-to-entity and entity-to-DTO mappings. Mappings are defined in `PlayerMappingProfile` under `Mappings/` and registered via `AddAutoMapper(typeof(PlayerMappingProfile))`. `PlayerRequestModel` does not expose an `Id` property, so callers structurally cannot set or override the internal primary key — no explicit `.ForMember(...Ignore())` is required on the `PlayerRequestModel → Player` mapping. The `Player → PlayerResponseModel` mapping suppresses the AutoMapper validation warning for the unmapped `Id` source member via `.ForSourceMember(source => source.Id, options => options.DoNotValidate())`.

## Consequences

### Positive
- Eliminates repetitive property-assignment boilerplate in service methods.
- Mapping configuration is centralised in one profile class, making it easy to audit what is and is not mapped.
- Protection of the internal `Id` is structural — the absence of `Id` on `PlayerRequestModel` is the guarantee, which is simpler and more robust than relying on an explicit ignore rule.

### Negative
- AutoMapper uses reflection at startup to validate mapping configurations, adding a small startup overhead.
- "Magic" convention-based mapping can be opaque: when a property is silently unmapped due to a name mismatch, the bug only surfaces at runtime (though `AssertConfigurationIsValid()` in tests can catch this).
- Adding AutoMapper as a dependency means contributors must understand profile configuration to modify or extend mappings.

### Neutral
- AutoMapper's `ProjectTo<T>()` LINQ extension for EF Core queryable projections is available but not currently used. Direct entity retrieval followed by in-memory mapping is sufficient at this project's scale.
