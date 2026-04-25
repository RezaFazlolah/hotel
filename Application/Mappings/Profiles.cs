using Application.Hotels.HotelCommands.HotelCommandRequests;
using Application.Hotels.HotelDtos;
using Application.Reservations.ReservationCommands.ReservationCommandRequests;
using Application.Reservations.ReservationDtos;
using Application.Rooms.RoomCommands.RoomCommandRequests;
using Application.Rooms.RoomDtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        // room
        CreateMap<Room, RoomDto>()
            .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<InsertRoomCommand, Room>();
        CreateMap<UpdateRoomCommand, Room>();
        // hotel
        CreateMap<Hotel, HotelDto>();
        // .ForMember(dst=>dst.RoomDtos,opt=>opt.MapFrom(src=>src.Rooms));
        CreateMap<InsertHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();
        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<InsertReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>().ReverseMap();
        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>().ReverseMap();
        // guest
    }
}