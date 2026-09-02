using Api.Dtos.RoomDtos;
using Application.Rooms.Commands;
using Application.Rooms.Filters;
using Application.Rooms.Queries;
using Application.Rooms.Sorts;
using AutoMapper;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class RoomMappingProfiles
    : Profile
{
    public RoomMappingProfiles()
    {
        CreateMap<CreateRoomCommandDto, CreateRoomCommand>();

        CreateMap<UpdateRoomBaseCommandDto, UpdateRoomBaseCommand>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .Include<UpdateRoomAsAdminCommandBaseDto, UpdateRoomAsAdminCommand>()
            .Include<UpdateRoomAsManagerCommandBaseDto, UpdateRoomAsManagerCommand>();
        CreateMap<UpdateRoomAsAdminCommandBaseDto, UpdateRoomAsAdminCommand>()
            .IncludeBase<UpdateRoomBaseCommandDto, UpdateRoomBaseCommand>();
        CreateMap<UpdateRoomAsManagerCommandBaseDto, UpdateRoomAsManagerCommand>()
            .IncludeBase<UpdateRoomBaseCommandDto, UpdateRoomBaseCommand>();

        CreateMap<GetAllRoomsQueryDto, GetAllRoomsQuery>()
            .ForMember(dst => dst.RoomFilterParameters,
                opt => opt.MapFrom(src =>
                    new RoomFilterParameters
                    {
                        MinNumber = src.MinNumber,
                        MaxNumber = src.MaxNumber,
                        Type = src.Type,
                        MinPricePerNight = src.MinPricePerNight,
                        MaxPricePerNight = src.MaxPricePerNight
                    }
                ))
            .ForMember(dst => dst.RoomSortParameters,
                opt => opt.MapFrom(src =>
                    src.SortBy.HasValue && src.IsAscending.HasValue
                        ? new RoomSortParameters
                        {
                            SortBy = src.SortBy.Value,
                            IsAscending = src.IsAscending.Value
                        }
                        : new RoomSortParameters()
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