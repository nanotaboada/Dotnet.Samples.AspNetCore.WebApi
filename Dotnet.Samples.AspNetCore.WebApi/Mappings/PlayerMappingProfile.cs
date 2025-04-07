using AutoMapper;
using Dotnet.Samples.AspNetCore.WebApi.Enums;
using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Mappings;

/// <summary>
/// Mapping profile for Player.
/// </summary>
/// <remarks>
/// This class defines the mapping configuration between PlayerRequestModel and Player,
/// and between Player and PlayerResponseModel.
/// </remarks>
public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        // PlayerRequestModel → Player
        CreateMap<PlayerRequestModel, Player>()
            .ForMember(
                destination => destination.Position,
                options =>
                    options.MapFrom(source =>
                        Position.FromAbbr(source.AbbrPosition ?? string.Empty)
                    )
            )
            .ForMember(destination => destination.Starting11, options => options.Ignore());

        // Player → PlayerResponseModel
        CreateMap<Player, PlayerResponseModel>()
            .ForMember(
                destination => destination.FullName,
                options =>
                    options.MapFrom(source =>
                        $"{source.FirstName} {(string.IsNullOrWhiteSpace(source.MiddleName) ? "" : source.MiddleName + " ")}{source.LastName}".Trim()
                    )
            )
            .ForMember(
                destination => destination.Birth,
                options => options.MapFrom(source => $"{source.DateOfBirth:MMMM d, yyyy}")
            )
            .ForMember(
                destination => destination.Dorsal,
                options => options.MapFrom(source => source.SquadNumber)
            )
            .ForMember(
                destination => destination.Club,
                options => options.MapFrom(source => source.Team)
            )
            .ForMember(
                destination => destination.Starting11,
                options => options.MapFrom(source => source.Starting11 ? "Yes" : "No")
            );
    }
}
