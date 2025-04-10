using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using FluentValidation;

namespace Dotnet.Samples.AspNetCore.WebApi.Validators;

public class PlayerRequestModelValidator : AbstractValidator<PlayerRequestModel>
{
    public PlayerRequestModelValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");

        RuleFor(x => x.SquadNumber)
            .NotEmpty()
            .WithMessage("SquadNumber is required.")
            .GreaterThan(0)
            .WithMessage("SquadNumber must be greater than 0.");

        RuleFor(x => x.AbbrPosition)
            .NotEmpty()
            .WithMessage("AbbrPosition is required.")
            .Must(Position.IsValidAbbr)
            .WithMessage("AbbrPosition is invalid.");
    }
}
