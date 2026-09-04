using Api.Dtos.AuthDtos;
using Application.Auth.Commands;
using AutoMapper;
using SharedKernel.Enums;

namespace Api.Mappings;

public class AuthMappingProfiles
    : Profile
{
    public AuthMappingProfiles()
    {
        CreateMap<RegisterCommandDto, RegisterCommand>()
            .ForMember(src=>src.Role,
                opt => opt.MapFrom(_ => UserRole.Guest));

        CreateMap<RegisterByAdminCommandDto, RegisterCommand>();
        
        CreateMap<LoginCommandDto, LoginCommand>();
    }
}