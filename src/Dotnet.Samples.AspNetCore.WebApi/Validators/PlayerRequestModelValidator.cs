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
/// This class is part of the FluentValidation library, which provides a fluent
/// interface for building validation rules.
/// </remarks>
public class PlayerRequestModelValidator : AbstractValidator<PlayerRequestModel>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerRequestModelValidator(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        RuleFor(player => player.FirstName).NotEmpty().WithMessage("FirstName is required.");

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
    }

    private async Task<bool> BeUniqueSquadNumber(
        int squadNumber,
        CancellationToken cancellationToken
    ) => (await _playerRepository.FindBySquadNumberAsync(squadNumber)) is null;
}
