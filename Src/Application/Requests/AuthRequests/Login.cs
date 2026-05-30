using MediatR;
using SharedKernel.Common;

namespace Application.Requests.AuthRequests;

public class Login : IRequest<Result<string>>
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
}