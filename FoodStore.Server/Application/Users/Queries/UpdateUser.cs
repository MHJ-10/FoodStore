using ErrorOr;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.Users.Queries;

public static class UpdateUser
{
   
    public class Request : IRequest<ErrorOr<Response>>
    {
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

    }
    public class Response
    {
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class Handler : IRequestHandler<Request, ErrorOr<Response>>
    {
        private readonly IUserService _userService;
        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserAsync(request, cancellationToken);
        }
    }
}