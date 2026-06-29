using Application.Interfaces.QueryServices;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.QueryServices;

public class ReservationQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Reservation, ReservationDto>(db, configurationProvider),
        IReservationQueryService
{
}