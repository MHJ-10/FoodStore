using ErrorOr;
using FluentValidation;
using FluentValidation.Validators;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Infrastructure;
using MediatR;
using Microsoft.CodeAnalysis.Scripting;

namespace FoodStore.Server.Application.Users.Commands;

public static class RegisterUser
{
    // =========================
    // Command
    // =========================
    public sealed record Request(
        string FirstName,
        string LastName,
        string UserName,
        string Password,
        string? PhoneNumber,
        string Email,
        string? Address
    ) : IRequest<ErrorOr<Response>>;

    // =========================
    // Validator
    // =========================
    public sealed class UserRequestValidator
        : AbstractValidator<Request>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                    .WithMessage("Email address format is invalid.")
                .MaximumLength(256);

            RuleFor(x => x.Address)
                .MinimumLength(10)
                    .WithMessage("Address must be at least 10 characters long.")
                .MaximumLength(500)
                    .WithMessage("Address cannot exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
        }
    }

    // =========================
    // Response
    // =========================
    public sealed class Response
    {
        public required string UserId { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
    }

    // =========================
    // Handler
    // =========================
    public sealed class Handler
        : IRequestHandler<Request, ErrorOr<Response>>
    {
        private readonly IUserService _userService;


        public Handler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _userService.RegisterAsync(request);
        }
    }
}
