using Application.Commands.HotelCommands;
using Application.Commands.ReservationCommands;
using Application.Commands.RoomCommands;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        // hotel
        CreateMap<InsertHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();

        // room
        CreateMap<InsertRoomCommand, Room>();
        CreateMap<UpdateRoomCommand, Room>();

        // reservation
        CreateMap<InsertReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
    }
}