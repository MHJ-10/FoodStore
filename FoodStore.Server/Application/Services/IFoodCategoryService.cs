using ErrorOr;
using FoodStore.Server.Application.FoodCategory.Commands;
using FoodStore.Server.Application.FoodCategory.Queries;
using FoodStore.Server.Infrastructure.DataModels;

namespace FoodStore.Server.Application.Services;

public interface IFoodCategoryService
{

    Task<ErrorOr<Success>> AddCategoryAsync(string name);
    Task<ErrorOr<IList<GetAllFoodCategories.Response>>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    Task<ErrorOr<GetFoodCategoryById.Response>> GetCategoryByIdAsync(int id);
    Task<ErrorOr<Success>> UpdateCategoryAsync(UpdateFoodCategory.Command request);
    Task<ErrorOr<Success>> DeleteCategoryAsync(int id);

}
