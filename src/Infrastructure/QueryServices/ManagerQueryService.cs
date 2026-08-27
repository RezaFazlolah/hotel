using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Application.Interfaces.QueryServices;
using Application.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class ManagerQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : QueryServiceBase<Domain.Models.Manager, ManagerDto>(db, configurationProvider),
        IManagerQueryService
{
}