using ErrorOr;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Domain.Valueobjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Application.Foods.Queries;

public static class GetAllFoods
{
    public class Request : IRequest<ErrorOr<IList<Response>>>
    {

    }
    public class Response
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public Money Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public byte[]? FoodImage { get; set; }
        public required string FoodCategoryName { get; set; }

    }

    public class Handler : IRequestHandler<Request, ErrorOr<IList<Response>>>
    {
        private readonly IFoodService _foodService;
        public Handler(IFoodService foodService)
        {
            _foodService = foodService;
        }
        public async Task<ErrorOr<IList<Response>>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _foodService.GetAllFoodsWithCategoriesAsync(cancellationToken);
        }
    }
}
