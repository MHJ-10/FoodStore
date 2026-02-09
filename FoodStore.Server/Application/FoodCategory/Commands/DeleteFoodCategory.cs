using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.FoodCategory.Commands;

public static class DeleteFoodCategory
{
    public sealed record Command(int Id) : IRequest<ErrorOr<Success>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Category Id have to be greater than 0");
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
            return await _service.DeleteCategoryAsync(request.Id);
        }
    }
}

