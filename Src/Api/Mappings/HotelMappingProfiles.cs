using Api.Dtos.HotelDtos;
using Application.Hotels.Commands;
using Application.Hotels.Queries;
using AutoMapper;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class HotelMappingProfiles
    : Profile
{
    public HotelMappingProfiles()
    {
        CreateMap<InsertHotelCommandDto, InsertHotelCommand>();

        CreateMap<UpdateHotelCommandDto, UpdateHotelCommand>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(_ => Guid.Empty));

        CreateMap<GetAllHotelsQueryDto, GetAllHotelsQuery>()
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