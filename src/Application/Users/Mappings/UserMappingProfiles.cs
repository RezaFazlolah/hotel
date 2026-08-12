using Application.Users.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Users.Mappings;

public class UserMappingProfiles
    : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<User, UserDto>()
            .ForMember(dst => dst.Roles, opt => opt.Ignore());

        CreateMap<Guest, GuestDto>()
            .ForMember(dst => dst.Roles, opt => opt.Ignore());

        CreateMap<Manager, ManagerDto>()
            .ForMember(dst => dst.Roles, opt => opt.Ignore());
    }
}