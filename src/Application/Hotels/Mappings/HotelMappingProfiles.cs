using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Hotels.Mappings;

public class HotelMappingProfiles
    : Profile
{
    public HotelMappingProfiles()
    {
        CreateMap<Hotel, HotelDto>()
            .ForMember(dst => dst.ManagerId,
                opt => opt.MapFrom(src =>
                    src.Manager != null
                        ? src.Manager.Id
                        : (Guid?)null));

        CreateMap<Result<Hotel>, Result<HotelDto>>();

        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>()
            .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data));

        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();

        CreateMap<InsertHotelCommand, Hotel>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.Manager, opt => opt.Ignore())
            .ForMember(dst => dst.Rooms, opt => opt.Ignore());

        CreateMap<UpdateHotelCommand, Hotel>()
            .ForMember(dst => dst.Manager, opt => opt.Ignore())
            .ForMember(dst => dst.Rooms, opt => opt.Ignore());
    }
}