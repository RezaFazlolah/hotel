using System.Runtime.InteropServices.JavaScript;
using Domain.Models;
using SharedKernel.Common;

namespace Domain.Interface;

public interface IReservationService
{
    public Task<Result<decimal>> CalculatePriceAsync(
        Reservation reservation,
        CancellationToken ct);
}