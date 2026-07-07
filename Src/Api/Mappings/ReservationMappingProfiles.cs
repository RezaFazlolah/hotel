using Api.Dtos.ReservationDtos;
using Application.Reservations.Commands;
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
                        : null)
            );
    }
}