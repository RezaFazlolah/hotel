using Api.Dtos.AuthDtos;
using Api.Dtos.HotelDtos;
using Api.Dtos.ReservationDtos;
using Api.Dtos.RoomDtos;
using Application.Auth.Commands;
using Application.Hotels.Commands;
using Application.Hotels.Queries;
using Application.Reservations.Commands;
using Application.Reservations.Queries;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class Profiles
    : Profile
{
    public Profiles()
    {
        // auth
        CreateMap<LoginCommandDto, LoginCommand>();

        CreateMap<RegisterCommandDto, RegisterCommand>()
            .ForCtorParam(
                nameof(RegisterCommand.Role),
                opt => opt.MapFrom(_ => UserRole.Guest));

        CreateMap<RegisterByAdminCommandDto, RegisterCommand>();

        // hotel
        CreateMap<InsertHotelCommandDto, InsertHotelCommand>();
        CreateMap<UpdateHotelCommandDto, UpdateHotelCommand>()
            .ForMember(dst => dst.Id, opt => opt.Ignore());
        CreateMap<GetAllHotelsQueryDto, GetAllHotelsQuery>()
            .ForCtorParam(nameof(GetAllHotelsQuery.PaginationParameters),
                opt => opt.MapFrom(src =>
                    new PaginationParameters
                    {
                        PageNumber = src.PageNumber,
                        PageSize = src.PageSize
                    }));

        // room
        CreateMap<InsertRoomCommandDto, InsertRoomCommand>();
        CreateMap<UpdateRoomCommandDto, UpdateRoomCommand>()
            .ForMember(dst => dst.Id, opt => opt.Ignore());
        CreateMap<GetAllRoomsQueryDto, GetAllRoomsQuery>()
            .ForCtorParam(nameof(GetAllRoomsQuery.PaginationParameters),
                opt => opt.MapFrom(src =>
                    new PaginationParameters
                    {
                        PageNumber = src.PageNumber,
                        PageSize = src.PageSize
                    }));
        
        // reservation
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>();
        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>()
            .ForMember(dst => dst.Id, opt => opt.Ignore());
        CreateMap<GetAllReservationsQueryDto, GetAllReservationsQuery>()
            .ForCtorParam(nameof(GetAllReservationsQuery.PaginationParameters),
                opt => opt.MapFrom(src =>
                    new PaginationParameters
                    {
                        PageNumber = src.PageNumber,
                        PageSize = src.PageSize
                    }));
    }
}