using Application.Requests.HotelRequests;
using Application.Requests.ReservationRequests;
using Application.Requests.RoomRequests;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        // hotel
        CreateMap<InsertHotel, Hotel>();
        CreateMap<UpdateHotel, Hotel>();

        // room
        CreateMap<InsertRoom, Room>();
        CreateMap<UpdateRoom, Room>();

        // reservation
        CreateMap<InsertReservation, Reservation>();
        CreateMap<UpdateReservation, Reservation>();
    }
}