using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using FluentValidation;

namespace Dotnet.Samples.AspNetCore.WebApi.Validators;

/// <summary>
/// Validator for PlayerRequestModel.
/// This class uses FluentValidation to define validation rules for the
/// PlayerRequestModel.
/// </summary>
/// <remarks>
/// Rules are organized into CRUD-named rule sets to make their intent explicit.
/// This prevents <c>BeUniqueSquadNumber</c> from running on PUT requests, where
/// the player's squad number already exists in the database by definition.
///
/// <list type="bullet">
///   <item><description>
///     <c>"Create"</c> — used by <c>POST /players</c>; includes all rules plus
///     the uniqueness check for <c>SquadNumber</c>.
///   </description></item>
///   <item><description>
///     <c>"Update"</c> — used by <c>PUT /players/squadNumber/{n}</c>; same
///     rules, but <c>BeUniqueSquadNumber</c> is intentionally omitted.
///   </description></item>
/// </list>
///
/// Controllers pass <c>opts.IncludeRuleSets("Create")</c> or
/// <c>opts.IncludeRuleSets("Update")</c> so that only the appropriate rule
/// set runs for each operation.
/// </remarks>
public class PlayerRequestModelValidator : AbstractValidator<PlayerRequestModel>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerRequestModelValidator(
        IPlayerRepository playerRepository,
        TimeProvider? timeProvider = null
    )
    {
        _playerRepository = playerRepository;
        var clock = timeProvider ?? TimeProvider.System;

        // "Create" rule set — POST /players
        // Includes BeUniqueSquadNumber to prevent duplicate squad numbers on insert.
        RuleSet(
            "Create",
            () =>
            {
                RuleFor(player => player.FirstName)
                    .NotEmpty()
                    .WithMessage("FirstName is required.");

                RuleFor(player => player.LastName).NotEmpty().WithMessage("LastName is required.");

                RuleFor(player => player.SquadNumber)
                    .NotEmpty()
                    .WithMessage("SquadNumber is required.")
                    .GreaterThan(0)
                    .WithMessage("SquadNumber must be greater than 0.")
                    .MustAsync(BeUniqueSquadNumber)
                    .WithMessage("SquadNumber must be unique.");

                RuleFor(player => player.AbbrPosition)
                    .NotEmpty()
                    .WithMessage("AbbrPosition is required.")
                    .Must(Position.IsValidAbbr)
                    .WithMessage("AbbrPosition is invalid.");

                When(
                    player => player.DateOfBirth.HasValue,
                    () =>
                    {
                        RuleFor(player => player.DateOfBirth)
                            .Must(date => date!.Value.Date < clock.GetUtcNow().Date)
                            .WithMessage("DateOfBirth must be a date in the past.")
                            .Must(date =>
                                date!.Value.Date
                                >= new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            )
                            .WithMessage("DateOfBirth must be on or after January 1, 1900.");
                    }
                );
            }
        );

        // "Update" rule set — PUT /players/squadNumber/{n}
        // BeUniqueSquadNumber is intentionally omitted: on PUT the player being
        // updated already exists in the database, so the check would always fail.
        RuleSet(
            "Update",
            () =>
            {
                RuleFor(player => player.FirstName)
                    .NotEmpty()
                    .WithMessage("FirstName is required.");

                RuleFor(player => player.LastName).NotEmpty().WithMessage("LastName is required.");

                RuleFor(player => player.SquadNumber)
                    .NotEmpty()
                    .WithMessage("SquadNumber is required.")
                    .GreaterThan(0)
                    .WithMessage("SquadNumber must be greater than 0.");

                RuleFor(player => player.AbbrPosition)
                    .NotEmpty()
                    .WithMessage("AbbrPosition is required.")
                    .Must(Position.IsValidAbbr)
                    .WithMessage("AbbrPosition is invalid.");

                When(
                    player => player.DateOfBirth.HasValue,
                    () =>
                    {
                        RuleFor(player => player.DateOfBirth)
                            .Must(date => date!.Value.Date < clock.GetUtcNow().Date)
                            .WithMessage("DateOfBirth must be a date in the past.")
                            .Must(date =>
                                date!.Value.Date
                                >= new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            )
                            .WithMessage("DateOfBirth must be on or after January 1, 1900.");
                    }
                );
            }
        );
    }

    private async Task<bool> BeUniqueSquadNumber(
        int squadNumber,
        CancellationToken cancellationToken
    ) => (await _playerRepository.FindBySquadNumberAsync(squadNumber)) is null;
}
