using Application.Interfaces.Services.Query;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.Services.Query;

public class RoomQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Room, RoomDto>(db, configurationProvider),
        IRoomQueryService
{
}