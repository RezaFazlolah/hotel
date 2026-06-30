using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Mappings;

public class ReservationMappingProfile
    : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>();
        // .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        CreateMap<Result<Reservation>, Result<ReservationDto>>();
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>();
        // .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();
        CreateMap<InsertReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
    }
}