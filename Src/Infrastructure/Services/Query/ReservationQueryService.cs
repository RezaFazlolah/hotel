using Application.Interfaces.Services.Query;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.Services.Query;

public class ReservationQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Reservation, ReservationDto>(db, configurationProvider),
        IReservationQueryService
{
}