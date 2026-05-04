using Api.DTOs.AuthDtos;
using Api.DTOs.HotelDtos;
using Api.DTOs.ReservationDtos;
using Api.DTOs.RoomDtos;
using Application.Commands.AuthCommands;
using Application.Commands.HotelCommands;
using Application.Commands.ReservationCommands;
using Application.Commands.RoomCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;

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
        CreateMap<Result<Hotel>, Result<HotelDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<Result<ICollection<Hotel>>, Result<ICollection<HotelDto>>>();
        CreateMap<InsertHotelCommandDto, InsertHotelCommand>();
        CreateMap<UpdateHotelCommandDto, UpdateHotelCommand>();

        // room
        CreateMap<Room, RoomDto>()
            .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<Result<ICollection<Room>>, Result<ICollection<RoomDto>>>();
        CreateMap<InsertRoomCommandDto, InsertRoomCommand>();
        CreateMap<UpdateRoomCommandDto, UpdateRoomCommand>();

        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<Result<Reservation>, Result<ReservationDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<Result<ICollection<Reservation>>, Result<ICollection<ReservationDto>>>();
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>();
        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>();
    }
}
