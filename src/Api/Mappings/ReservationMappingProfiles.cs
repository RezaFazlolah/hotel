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
        CreateMap<CreateReservationCommandDto, CreateReservationCommand>();

        CreateMap<UpdateReservationBaseCommandDto, UpdateReservationBaseCommand>()
            .ForMember(dst => dst.ReservationId, opt => opt.Ignore())
            .Include<UpdateReservationAsAdminCommandDto, UpdateReservationAsAdminCommand>()
            .Include<UpdateReservationAsManagerCommandDto, UpdateReservationAsManagerCommand>()
            .Include<UpdateReservationAsGuestCommandDto, UpdateReservationAsGuestCommand>();
        CreateMap<UpdateReservationAsAdminCommandDto, UpdateReservationAsAdminCommand>()
            .IncludeBase<UpdateReservationBaseCommandDto, UpdateReservationBaseCommand>();
        CreateMap<UpdateReservationAsManagerCommandDto, UpdateReservationAsManagerCommand>()
            .IncludeBase<UpdateReservationBaseCommandDto, UpdateReservationBaseCommand>();
        CreateMap<UpdateReservationAsGuestCommandDto, UpdateReservationAsGuestCommand>()
            .IncludeBase<UpdateReservationBaseCommandDto, UpdateReservationBaseCommand>();
        
        CreateMap<GetAllReservationsQueryDto, GetAllReservationsQuery>()
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
                ))
            .ForMember(dst => dst.PaginationParameters,
                opt => opt.MapFrom(src =>
                    src.PageNumber.HasValue && src.PageSize.HasValue
                        ? new PaginationParameters
                        {
                            PageNumber = src.PageNumber.Value,
                            PageSize = src.PageSize.Value
                        }
                        : new PaginationParameters())
            );
    }
}