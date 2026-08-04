using Api.Dtos.ReservationDtos;
using Application.Reservations.Commands;
using Application.Reservations.Filters;
using Application.Reservations.Queries;
using AutoMapper;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class ReservationMappingProfiles
    : Profile
{
    public ReservationMappingProfiles()
    {
        CreateMap<InsertReservationCommandDto, InsertReservationCommand>();

        CreateMap<UpdateReservationCommandDto, UpdateReservationCommand>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(_ => Guid.Empty));

        CreateMap<GetAllReservationsQueryDto, GetAllReservationsQuery>()
            .ForMember(dst => dst.PaginationParameters,
                opt => opt.MapFrom(src =>
                    src.PageNumber.HasValue && src.PageSize.HasValue
                        ? new PaginationParameters
                        {
                            PageNumber = src.PageNumber.Value,
                            PageSize = src.PageSize.Value
                        }
                        : new PaginationParameters())
            )
            .ForMember(dst => dst.ReservationFilterParameters,
                opt => opt.MapFrom(src =>
                    new ReservationFilterParameters
                    {
                        MinCheckInDate = src.MinCheckInDate,
                        MaxCheckInDate = src.MaxCheckInDate,
                        MinCheckOutDate = src.MinCheckOutDate,
                        MaxCheckOutDate = src.MaxCheckOutDate,
                        MinTotalPrice = src.MinTotalPrice,
                        MaxTotalPrice = src.MaxTotalPrice,
                        Status = src.Status
                    }));
    }
}