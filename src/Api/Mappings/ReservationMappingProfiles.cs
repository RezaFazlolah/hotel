using Api.Dtos.ReservationDtos;
using Application.Reservations.Commands;
using Application.Reservations.Filters;
using Application.Reservations.Queries;
using Application.Reservations.Sorts;
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
                    }))
            .ForMember(dst => dst.ReservationSortParameters,
                opt => opt.MapFrom(src =>
                    src.SortBy.HasValue && src.IsAscending.HasValue
                        ? new ReservationSortParameters
                        {
                            SortBy = src.SortBy.Value,
                            IsAscending = src.IsAscending.Value
                        }
                        : new ReservationSortParameters()
                ));
    }
}