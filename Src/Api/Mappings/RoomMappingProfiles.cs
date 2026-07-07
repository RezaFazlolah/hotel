using Api.Dtos.RoomDtos;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using SharedKernel.Paginations;

namespace Api.Mappings;

public class RoomMappingProfiles
    : Profile
{
    public RoomMappingProfiles()
    {
        CreateMap<InsertRoomCommandDto, InsertRoomCommand>();

        CreateMap<UpdateRoomCommandDto, UpdateRoomCommand>()
            .ForMember(dst => dst.Id, opt => opt.Ignore());

        CreateMap<GetAllRoomsQueryDto, GetAllRoomsQuery>()
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