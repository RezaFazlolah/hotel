using Api.DTOs.AuthDtos;
using Api.DTOs.HotelDtos;
using Api.DTOs.ReservationDtos;
using Api.DTOs.RoomDtos;
using Application.Auth.Commands;
using Application.Hotels.Commands;
using Application.Hotels.Queries;
using Application.Reservations.Commands;
using Application.Reservations.Queries;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Api.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        // auth
        CreateMap<LoginCommandDto, LoginCommand>();
        CreateMap<RegisterCommandDto, RegisterCommand>();
        CreateMap<RegisterByAdminCommandDto, RegisterCommand>();
        CreateMap<User, UserDto>();
        CreateMap<Result<User>, Result<UserDto>>();

        // hotel
        CreateMap<Hotel, HotelDto>();
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelCommandDto, InsertHotelCommand>();
        CreateMap<UpdateHotelCommandDto, UpdateHotelCommand>();
        CreateMap<GetAllHotelsQueryDto, GetAllHotelsQuery>();

        // room
        CreateMap<Room, RoomDto>();
        // .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>();
        CreateMap<PagedResult<Room>, PagedResult<RoomDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Room>>, Result<PagedResult<RoomDto>>>();
        CreateMap<InsertRoomCommandDto, InsertRoomCommand>();
        CreateMap<UpdateRoomCommandDto, UpdateRoomCommand>().ReverseMap();
        CreateMap<UpdateRoomCommand, Room>().ReverseMap();
        CreateMap<GetAllRoomsQueryDto, GetAllRoomsQuery>();

        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>();
        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>();
        CreateMap<GetAllReservationsQueryDto, GetAllReservationsQuery>();
    }
}