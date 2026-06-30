using Application.Auth.Commands;
using Application.Auth.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class AuthMappingProfile
    : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, RegisteredUserDto>();
        CreateMap<User, LoggedinUserDto>();
        CreateMap<RegisterCommand, User>()
            .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.PhoneNumber));
    }
}