using Azure;
using ErrorOr;
using FoodStore.Server.Application.Services;
using MediatR;

namespace FoodStore.Server.Application.FoodCategory.Queries;

public static class GetAllFoodCategories
{
    public sealed record Query() : IRequest<ErrorOr<IList<Response>>>;

    public sealed record Response(int Id, string Name);

    public sealed class Handler : IRequestHandler<Query, ErrorOr<IList<Response>>>
    {
        private readonly IFoodCategoryService _service;

        public Handler(IFoodCategoryService service)
        {
            _service = service;
        }

        public async Task<ErrorOr<IList<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _service.GetAllCategoriesAsync(cancellationToken);
        }
    }
}

