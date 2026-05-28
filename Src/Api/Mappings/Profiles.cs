using Api.DTOs.AuthDtos;
using Api.DTOs.HotelDtos;
using Api.DTOs.ReservationDtos;
using Api.DTOs.RoomDtos;
using Application.Commands.AuthCommands;
using Application.Commands.HotelCommands;
using Application.Commands.ReservationCommands;
using Application.Commands.RoomCommands;
using Application.Queries.HotelQueries;
using Application.Queries.ReservationQueries;
using Application.Queries.RoomQueries;
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
        CreateMap<Result<Hotel>, Result<HotelDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelDto, InsertHotel>();
        CreateMap<UpdateHotelDto, UpdateHotel>();
        CreateMap<GetAllHotelsDto, GetAllHotels>()
            .ForMember(dst => dst.PaginationParameters,
                opt => opt.MapFrom(src => new PaginationParameters
                    { PageNumber = src.PageNumber, PageSize = src.PageSize }));

        // room
        CreateMap<Room, RoomDto>()
            .ForMember(dst => dst.HotelDto, opt => opt.MapFrom(src => src.Hotel));
        CreateMap<Result<Room>, Result<RoomDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<Result<ICollection<Room>>, Result<ICollection<RoomDto>>>();
        CreateMap<InsertRoomDto, InsertRoom>();
        CreateMap<UpdateRoomDto, UpdateRoom>();
        CreateMap<GetAllRoomsDto, GetAllRooms>();
        
        // reservation
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<Result<Reservation>, Result<ReservationDto>>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value));
        CreateMap<Result<ICollection<Reservation>>, Result<ICollection<ReservationDto>>>();
        CreateMap<InsertReservationDto, InsertReservation>();
        CreateMap<UpdateReservationDto, UpdateReservation>();
        CreateMap<GetAllReservationsDto, GetAllReservations>();
    }
}