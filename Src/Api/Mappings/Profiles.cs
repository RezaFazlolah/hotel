using Api.DTOs.AuthDtos;
using Api.DTOs.HotelDtos;
using Api.DTOs.ReservationDtos;
using Api.DTOs.RoomDtos;
using Application.Requests.AuthRequests;
using Application.Requests.HotelRequests;
using Application.Requests.ReservationRequests;
using Application.Requests.RoomRequests;
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
        CreateMap<LoginDto, Login>();
        CreateMap<RegisterDto, Register>();
        CreateMap<RegisterByAdminDto, Register>();
        CreateMap<User, UserDto>();
        CreateMap<Result<User>, Result<UserDto>>();

        // hotel
        CreateMap<Hotel, HotelDto>();
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelDto, InsertHotel>();
        CreateMap<UpdateHotelDto, UpdateHotel>();
        CreateMap<GetAllHotelsDto, GetAllHotels>();

        // room
        CreateMap<Room, RoomDto>();
            // .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>();
        CreateMap<PagedResult<Room>, PagedResult<RoomDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Room>>, Result<PagedResult<RoomDto>>>();
        CreateMap<InsertRoomDto, InsertRoom>();
        CreateMap<UpdateRoomDto, UpdateRoom>();
        CreateMap<GetAllRoomsDto, GetAllRooms>();

        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();
        CreateMap<InsertReservationDto, InsertReservation>();
        CreateMap<UpdateReservationDto, UpdateReservation>();
        CreateMap<GetAllReservationsDto, GetAllReservations>();
    }
}