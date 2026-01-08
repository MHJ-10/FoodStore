using ErrorOr;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.Users.Commands;

public static class RevokeRefreshToken
{
    public sealed record Request(string UserId) : IRequest<ErrorOr<Success>>
    {
    }
    public sealed class Handler : IRequestHandler<Request, ErrorOr<Success>>
    {
        private readonly IUserService _userService;
        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ErrorOr<Success>> Handle(Request request, CancellationToken cancellationToken)
        {
          return  await _userService.RevokeRefreshTokenAsync(request);
        }
    }
}
