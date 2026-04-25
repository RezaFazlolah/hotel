using Api.DTOs.AuthDTOs;
using Application.Commands.AuthCommands;
using AutoMapper;

namespace Api.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<RegisterCommandDto, RegisterCommand>();
        CreateMap<LoginCommandDto, LoginCommand>();
    }
}
