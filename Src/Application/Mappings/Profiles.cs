using Api.Dtos.HotelDtos;
using Application.Dtos.Auth;
using Application.Dtos.ReservationDtos;
using Application.Dtos.RoomDtos;
using Application.Hotels.Commands;
using Application.Reservations.Commands;
using Application.Rooms.Commands;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        // Auth
        CreateMap<User, UserDto>();
        CreateMap<Result<User>, Result<UserDto>>();

        // hotel
        CreateMap<Hotel, HotelDto>();
        CreateMap<Result<Hotel>, Result<HotelDto>>();
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();

        // room
        CreateMap<Room, RoomDto>();
        // .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>();
        CreateMap<PagedResult<Room>, PagedResult<RoomDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Room>>, Result<PagedResult<RoomDto>>>();
        CreateMap<InsertRoomCommand, Room>();
        CreateMap<UpdateRoomCommand, Room>();

        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();
        CreateMap<InsertReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
    }
}