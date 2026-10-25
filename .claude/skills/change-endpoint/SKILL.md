---
name: change-endpoint
description: Add a new endpoint or change an existing one (route, HTTP status codes, request/response shape, validation rules) end to end across controller, service, repository, validator, tests, and docs. Use for any API behavior change in this project.
---

# Change an endpoint

The API contract (endpoints, status codes, response shapes) is an invariant:
confirm the change with the user and make sure an issue exists before editing.

All paths below are relative to `src/Dotnet.Samples.AspNetCore.WebApi/` unless
they start with `test/`.

1. **Models** — add or update DTOs in `Models/` (`PlayerRequestModel`,
   `PlayerResponseModel`); keep the `Player` entity untouched unless the
   schema changes too (then also follow the `change-data` skill).
2. **Mapping** — update `Mappings/PlayerMappingProfile.cs`.
3. **Repository** — add query methods to `IPlayerRepository` /
   `PlayerRepository` only if `Repository<T>` does not already cover them.
   Use `AsNoTracking()` for reads.
4. **Service** — add or update the method in `IPlayerService` /
   `PlayerService`; keep cache invalidation consistent with the other
   mutating methods.
5. **Validation** — update `Validators/PlayerRequestModelValidator.cs` inside
   the `Create` or `Update` rule set; the controller must call the matching
   rule set explicitly.
6. **Controller** — add or update the action in
   `Controllers/PlayerController.cs`:
   - `[Http*(..., Name = "...")]` route, `/// <summary>` XML docs (they feed
     Swagger)
   - one `[ProducesResponseType]` per possible status code
   - `TypedResults.*`; RFC 7807 Problem Details for errors, `422` for
     validation failures
7. **Tests** — follow the naming patterns in `CLAUDE.md`:
   - `test/.../Unit/PlayerControllerTests.cs`, `PlayerServiceTests.cs`,
     `PlayerValidatorTests.cs`
   - `test/.../Integration/PlayerWebApplicationTests.cs` for the HTTP-level
     status code and payload
   - build test data with `PlayerFakes` / `PlayerMocks`
8. **Docs** — update the API Reference table and error codes in `README.md`;
   add an ADR (see the `create-adr` skill) if the change alters an accepted
   decision such as status codes or the mutation key.
9. Run `/pre-commit`.
