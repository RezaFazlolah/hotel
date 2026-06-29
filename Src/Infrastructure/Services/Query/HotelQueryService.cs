using Application.Hotels.Dtos;
using Application.Interfaces.Services.Query;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.Services.Query;

public class HotelQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Hotel, HotelDto>(db, configurationProvider),
        IHotelQueryService
{
}