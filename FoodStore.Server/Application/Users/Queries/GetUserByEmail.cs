using ErrorOr;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.Users.Queries;

public static class GetUserByEmail
{
    public sealed record Request(string Email) : IRequest<ErrorOr<Response>>
    {
    }
    public class Response
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
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
            return await _userService.GetUserByEmailAsync(request.Email, cancellationToken);
        }
    }
}