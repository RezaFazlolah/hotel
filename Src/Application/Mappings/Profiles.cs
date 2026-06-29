using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Mappings;

public class AuthProfiles
    : Profile
{
    public AuthProfiles()
    {
        CreateMap<User, RegisteredUserDto>();
        CreateMap<User, LoggedinUserDto>();
        CreateMap<RegisterCommand, User>()
            .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.PhoneNumber));
    }
}

public class HotelProfiles
    : Profile
{
    public HotelProfiles()
    {
        CreateMap<Hotel, HotelDto>();
        CreateMap<Result<Hotel>, Result<HotelDto>>();
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();
    }
}

public class RoomProfiles
    : Profile
{
    public RoomProfiles()
    {
        CreateMap<Room, RoomDto>();
        // .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>();
        CreateMap<PagedResult<Room>, PagedResult<RoomDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Room>>, Result<PagedResult<RoomDto>>>();
        CreateMap<InsertRoomCommand, Room>();
        CreateMap<UpdateRoomCommand, Room>();
    }
}

public class ReservationProfiles
    : Profile
{
    public ReservationProfiles()
    {
        CreateMap<Reservation, ReservationDto>();
        // .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<Result<Reservation>, Result<ReservationDto>>();
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>();
        // .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();
        CreateMap<InsertReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
    }
}