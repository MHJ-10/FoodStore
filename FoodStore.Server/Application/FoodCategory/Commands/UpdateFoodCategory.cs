using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.FoodCategory.Commands;

public static class UpdateFoodCategory
{
    public sealed record Command(int Id, string Name) : IRequest<ErrorOr<Success>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Category Id can not be null");
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Category name is required.")
               .MinimumLength(2).WithMessage("Category name must be at least 2 characters long.")
               .MaximumLength(50).WithMessage("Category name must not exceed 50 characters.");
        }
    }

    public sealed class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IFoodCategoryService _service;

        public Handler(IFoodCategoryService service)
        {
            _service = service;
        }

        public async Task<ErrorOr<Success>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _service.UpdateCategoryAsync(request);
        }
    }
}
