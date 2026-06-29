using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IHotelRepository
    : IBaseRepository<Guid, Hotel>
{
}