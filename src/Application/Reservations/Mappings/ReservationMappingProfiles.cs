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
        // .ForMember(dst => dst.RoomDto, opt => opt.MapFrom(src => src.Room));
        
        CreateMap<Result<Reservation>, Result<ReservationDto>>();
        
        CreateMap<PagedResult<Reservation>, PagedResult<ReservationDto>>();
        // .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        
        CreateMap<Result<PagedResult<Reservation>>, Result<PagedResult<ReservationDto>>>();

        CreateMap<InsertReservationCommand, Reservation>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.TotalPrice, opt => opt.Ignore())
            .ForMember(dst => dst.Status, opt => opt.Ignore())
            .ForMember(dst => dst.Guest, opt => opt.Ignore())
            .ForMember(dst => dst.Room, opt => opt.Ignore());
        
        CreateMap<UpdateReservationCommand, Reservation>()
            .ForMember(dst=>dst.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dst => dst.GuestId, opt => opt.Ignore())
            .ForMember(dst => dst.RoomId, opt => opt.Ignore())
            .ForMember(dst => dst.TotalPrice, opt => opt.Ignore())
            .ForMember(dst => dst.Status, opt => opt.Ignore())
            .ForMember(dst => dst.Guest, opt => opt.Ignore())
            .ForMember(dst => dst.Room, opt => opt.Ignore());
    }
}