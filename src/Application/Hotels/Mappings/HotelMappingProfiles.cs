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
        CreateMap<Hotel, BaseHotelDto>();
        CreateMap<Hotel, HotelDto>()
            .ForMember(dst => dst.ManagerId,
                opt => opt.MapFrom(src =>
                    src.Manager != null
                        ? src.Manager.Id
                        : (Guid?)null))
            .IncludeBase<Hotel, BaseHotelDto>();
        CreateMap<Hotel, UpdatedHotelDto>()
            .IncludeBase<Hotel, BaseHotelDto>();

        CreateMap<Result<Hotel>, Result<HotelDto>>();
        CreateMap<Result<Hotel>, Result<UpdatedHotelDto>>();

        CreateMap<PagedResult<Hotel>, PagedResult<HotelDto>>();
        CreateMap<PagedResult<Hotel>, PagedResult<UpdatedHotelDto>>();

        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<HotelDto>>>();
        CreateMap<Result<PagedResult<Hotel>>, Result<PagedResult<UpdatedHotelDto>>>();

        CreateMap<CreateHotelCommand, Hotel>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.Manager, opt => opt.Ignore())
            .ForMember(dst => dst.Rooms, opt => opt.Ignore());

        CreateMap<UpdateHotelCommandBase, Hotel>()
            .ForMember(dst => dst.Rating, opt => opt.Ignore())
            .ForMember(dst => dst.Manager, opt => opt.Ignore())
            .ForMember(dst => dst.Rooms, opt => opt.Ignore())
            .Include<UpdateHotelAsAdminCommand, Hotel>()
            .Include<UpdateHotelAsManagerCommand, Hotel>();
        CreateMap<UpdateHotelAsAdminCommand, Hotel>()
            .ForMember(dst => dst.Rating, opt => opt.MapFrom(src => src.Rating))
            .IncludeBase<UpdateHotelCommandBase, Hotel>();
        CreateMap<UpdateHotelAsManagerCommand, Hotel>()
            .IncludeBase<UpdateHotelCommandBase, Hotel>();
    }
}