using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Mappings;

public class HotelMappingProfile
    : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, HotelDto>();
        CreateMap<Result<Hotel>, Result<HotelDto>>();
        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<InsertHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();
    }
}