using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Reservations.Mappings;

public class ReservationMappingProfiles
    : Profile
{
    public ReservationMappingProfiles()
    {
        CreateMap<Reservation, ReservationDto>();

        CreateMap<Result<Reservation>, Result<ReservationDto>>();
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>();
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();

        CreateMap<CreateReservationCommand, Reservation>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.TotalPrice, opt => opt.Ignore())
            .ForMember(dst => dst.Status, opt => opt.Ignore())
            .ForMember(dst => dst.Guest, opt => opt.Ignore())
            .ForMember(dst => dst.Room, opt => opt.Ignore());

        CreateMap<UpdateReservationBaseCommand, Reservation>()
            .ForMember(dst=>dst.Id, opt => opt.MapFrom(src=>src.ReservationId))
            .ForMember(dst=>dst.TotalPrice, opt => opt.Ignore())
            .ForMember(dst=>dst.Status, opt => opt.Ignore())
            .ForMember(dst=>dst.GuestId, opt => opt.Ignore())
            .ForMember(dst=>dst.Guest, opt => opt.Ignore())
            .ForMember(dst=>dst.RoomId, opt => opt.Ignore())
            .ForMember(dst=>dst.Room, opt => opt.Ignore())
            .Include<UpdateReservationAsAdminCommand, Reservation>()
            .Include<UpdateReservationAsManagerCommand, Reservation>()
            .Include<UpdateReservationAsGuestCommand, Reservation>();
        CreateMap<UpdateReservationAsAdminCommand, Reservation>()
            .IncludeBase<UpdateReservationBaseCommand, Reservation>();
        CreateMap<UpdateReservationAsManagerCommand, Reservation>()
            .IncludeBase<UpdateReservationBaseCommand, Reservation>();
        CreateMap<UpdateReservationAsGuestCommand, Reservation>()
            .IncludeBase<UpdateReservationBaseCommand, Reservation>();
    }
}