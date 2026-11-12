using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using FluentValidation;

namespace Dotnet.Samples.AspNetCore.WebApi.Validators;

public sealed class PlayerSearchRequestValidator
    : AbstractValidator<PlayerSearchRequest>
{
    public PlayerSearchRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .WithMessage(
                "Page must be greater than 0.");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(
                "PageSize must be between 1 and 100.");

        RuleFor(request => request.Position)
            .Must(Position.IsValidAbbr)
            .When(request =>
                !string.IsNullOrWhiteSpace(
                    request.Position))
            .WithMessage(
                "Position is invalid.");
    }
}