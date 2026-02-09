using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Services;
using MediatR;

public static class AddFoodCategory
{
    // -------------------------
    // COMMAND
    // -------------------------
    public class Command : IRequest<ErrorOr<Success>>
    {
        public required string Name { get; set; }
    }

    // -------------------------
    // VALIDATOR
    // -------------------------
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Category name must not exceed 50 characters.");
        }
    }

    // -------------------------
    // HANDLER
    // -------------------------
    public sealed class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public Handler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public async Task<ErrorOr<Success>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _foodCategoryService.AddCategoryAsync(request.Name);

        }
    }
}
