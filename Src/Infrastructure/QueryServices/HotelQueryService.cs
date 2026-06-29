using Application.Hotels.Dtos;
using Application.Interfaces.QueryServices;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.QueryServices;

public class HotelQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Hotel, HotelDto>(db, configurationProvider),
        IHotelQueryService
{
}