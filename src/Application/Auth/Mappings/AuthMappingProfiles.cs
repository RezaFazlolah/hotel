using Application.Auth.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Auth.Mappings;

public class AuthMappingProfiles
    : Profile
{
    public AuthMappingProfiles()
    {
        CreateMap<User, BaseUserDto>()
            .ForMember(dst => dst.Roles, opt => opt.Ignore());

        CreateMap<User, RegisteredUserDto>()
            .IncludeBase<User, BaseUserDto>();
            
        CreateMap<User, LoggedinUserDto>()
            .ForMember(dst=>dst.Jwt, opt=>opt.Ignore())
            .IncludeBase<User, BaseUserDto>();
    }
}