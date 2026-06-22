using Api.Dtos.AuthDtos;
using Api.Dtos.HotelDtos;
using Api.Dtos.ReservationDtos;
using Api.Dtos.RoomDtos;
using Application.Auth.Commands;
using Application.Dtos.Auth;
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

        // hotel
        CreateMap<InsertHotelCommandDto, InsertHotelCommand>();
        CreateMap<GetAllHotelsQueryDto, GetAllHotelsQuery>();

        // room
        CreateMap<InsertRoomCommandDto, InsertRoomCommand>();
        CreateMap<UpdateRoomCommandDto, UpdateRoomCommand>().ReverseMap();
        CreateMap<UpdateRoomCommand, Room>().ReverseMap();
        CreateMap<GetAllRoomsQueryDto, GetAllRoomsQuery>();

        // reservation
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>();
        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>();
        CreateMap<GetAllReservationsQueryDto, GetAllReservationsQuery>();
    }
}