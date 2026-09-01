using Api.Dtos.HotelDtos;
using Application.Hotels.Commands;
using Application.Hotels.Filters;
using Application.Hotels.Queries;
using Application.Hotels.Sorts;
using AutoMapper;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class HotelMappingProfiles
    : Profile
{
    public HotelMappingProfiles()
    {
        CreateMap<CreateHotelCommandDto, CreateHotelCommand>();

        CreateMap<UpdateHotelCommandBaseDto, UpdateHotelCommandBase>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(_ => Guid.Empty))
            .Include<UpdateHotelAsAdminCommandDto, UpdateHotelAsAdminCommand>()
            .Include<UpdateHotelAsManagerCommandDto, UpdateHotelAsManagerCommand>();
        CreateMap<UpdateHotelAsAdminCommandDto, UpdateHotelAsAdminCommand>()
            .IncludeBase<UpdateHotelCommandBaseDto, UpdateHotelCommandBase>();
        CreateMap<UpdateHotelAsManagerCommandDto, UpdateHotelAsManagerCommand>()
            .IncludeBase<UpdateHotelCommandBaseDto, UpdateHotelCommandBase>();
        
        
        CreateMap<GetAllHotelsQueryDto, GetAllHotelsQuery>()
            .ForMember(dst => dst.HotelFilterParameters,
                opt => opt.MapFrom(src =>
                    new HotelFilterParameters
                    {
                        Name = src.Name,
                        Address = src.Address,
                        MinRating = src.MinRating,
                        MaxRating = src.MaxRating
                    }))
            .ForMember(dst => dst.HotelSortParameters,
                opt => opt.MapFrom(src =>
                    src.SortBy.HasValue && src.IsAscending.HasValue
                        ? new HotelSortParameters
                        {
                            SortBy = src.SortBy.Value,
                            IsAscending = src.IsAscending.Value
                        }
                        : new HotelSortParameters()
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