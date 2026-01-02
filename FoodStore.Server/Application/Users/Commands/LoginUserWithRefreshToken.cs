using Azure;
using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.Users.Commands;

public static class LoginUserWithRefreshToken
{
    public sealed record Request(string RefreshToken) : IRequest<ErrorOr<Response>>
    {
        
    }
    public sealed class LoginUserWithRefreshTokenRequestValidator : AbstractValidator<Request>
    {
        public LoginUserWithRefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(256).WithMessage("Maximum Refresh token is 256 characters");
        }
    }
    public record Response(string AccessToken, string RefreshToken);
    public sealed class Handler : IRequestHandler<Request, ErrorOr<Response>>
    {
        private readonly IUserService _userService;
        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
           return await _userService.LoginUserWithRefreshTokenAsync(request);
        }
    }

}
