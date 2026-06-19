using Application.Hotels.Commands;
using Application.Reservations.Commands;
using Application.Rooms.Commands;
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