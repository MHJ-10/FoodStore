using ErrorOr;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Presentation.ViewModels;
using MediatR;

namespace FoodStore.Server.Application.FoodCategory.Queries;

public static class GetFoodCategoryById
{
    public sealed record Query(int Id) : IRequest<ErrorOr<Response>>;

    public sealed class Response()
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IList<FoodViewModel> ?Foods { get; set; }
    }

    public sealed class Handler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly IFoodCategoryService _service;

        public Handler(IFoodCategoryService service)
        {
            _service = service;
        }

        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _service.GetCategoryByIdAsync(request.Id);
        }
    }
}

