using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.Users.Commands
{
    public static class ConfirmEmail
    {
        public sealed record Request(string UserId, string Code) : IRequest<ErrorOr<Success>>;

        public sealed class ConfirmEmailValidator : AbstractValidator<Request>
        {
            public ConfirmEmailValidator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
                RuleFor(x => x.Code)
                 .NotEmpty().WithMessage("Code is required.");
            }
        }
        public sealed class Handler : IRequestHandler<Request, ErrorOr<Success>>
        {
            private readonly IUserService _userService;
            public Handler(IUserService userService)
            {
                _userService = userService;
            }
            async Task<ErrorOr<Success>> IRequestHandler<Request, ErrorOr<Success>>.Handle(Request request, CancellationToken cancellationToken)
            {
                return await _userService.ConfirmEmailAsync(request);
            }
        }
    }
}
