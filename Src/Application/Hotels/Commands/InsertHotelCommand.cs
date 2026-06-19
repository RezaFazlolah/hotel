using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record InsertHotelCommand(Guid Id, string Name, string Address, float Rating)
    : IRequest<Result<Domain.Models.Hotel>>;