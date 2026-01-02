using Azure;
using ErrorOr;
using FluentValidation;
using FluentValidation.Validators;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Domain.Enums;
using MediatR;

namespace FoodStore.Server.Application.Users.Commands;

public static class AddRole
{
    public sealed record Request(string Email, string Password, string Role) : IRequest<ErrorOr<Success>>;
    public sealed class AddRoleRequestValidator : AbstractValidator<Request>
    {
        public AddRoleRequestValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email address is required.")
             .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                 .WithMessage("Email address format is invalid.")
             .MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required");
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
            return await _userService.AddRoleAsync(request);
        }
    }

}

