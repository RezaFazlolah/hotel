using Application.Auth.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Auth.Mappings;

public class AuthMappingProfiles
    : Profile
{
    public AuthMappingProfiles()
    {
        CreateMap<User, UserDto>()
            .ForMember(dst => dst.Roles, opt => opt.Ignore())
            .Include<Admin, AdminDto>()
            .Include<Manager, ManagerDto>()
            .Include<Guest, GuestDto>();
        CreateMap<Admin, AdminDto>()
            .IncludeBase<User, UserDto>();
        CreateMap<Manager, ManagerDto>()
            .IncludeBase<User, UserDto>();
        CreateMap<Guest, GuestDto>()
            .IncludeBase<User, UserDto>();


        CreateMap<User, RegisteredUserDto>()
            .IncludeBase<User, UserDto>();

        CreateMap<User, LoggedinUserDto>()
            .ForMember(dst => dst.Jwt, opt => opt.Ignore())
            .IncludeBase<User, UserDto>();
    }
}