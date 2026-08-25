using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Rooms.Mappings;

public class RoomMappingProfiles
    : Profile
{
    public RoomMappingProfiles()
    {
        CreateMap<Room, RoomDto>();
        // .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        
        CreateMap<Result<Room>, Result<RoomDto>>();
        
        CreateMap<PagedResult<Room>, PagedResult<RoomDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
       
        CreateMap<Result<PagedResult<Room>>, Result<PagedResult<RoomDto>>>();
        
        CreateMap<CreateRoomCommand, Room>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.Hotel, opt => opt.Ignore())
            .ForMember(dst => dst.Reservations, opt => opt.Ignore());
        
        
        CreateMap<UpdateRoomCommand, Room>()
            .ForMember(dst => dst.Hotel, opt => opt.Ignore())
            .ForMember(dst => dst.Reservations, opt => opt.Ignore());
    }
}