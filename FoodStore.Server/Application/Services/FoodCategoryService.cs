using ErrorOr;
using FoodStore.Server.Application.FoodCategory.Commands;
using FoodStore.Server.Application.FoodCategory.Errors;
using FoodStore.Server.Application.FoodCategory.Queries;
using FoodStore.Server.Infrastructure;
using FoodStore.Server.Infrastructure.DataModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Server.Application.Services;

public class FoodCategoryService: IFoodCategoryService
{
    private readonly FoodStoreDbContext _foodStoreDbContext;
    public FoodCategoryService(FoodStoreDbContext foodStoreDbContext)
    {
            _foodStoreDbContext = foodStoreDbContext;
    }
    // ADD CATEGORY
    public async Task<ErrorOr<Success>> AddCategoryAsync(string name)
    {
        // 1. Check duplicates
        bool exists = await _foodStoreDbContext.FoodCategories
            .AnyAsync(c => c.Name.ToLower() == name.ToLower());

        if (exists)
            return CategoryErrors.DuplicateName(name);

        // 2. Create entity
        var category = new Infrastructure.DataModels.FoodCategory
        {
            Name = name
        };

        _foodStoreDbContext.FoodCategories.Add(category);

        // 3. Save
        int saveResult = await _foodStoreDbContext.SaveChangesAsync();

        if (saveResult == 0)
            return CategoryErrors.FailedToCreate;

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> DeleteCategoryAsync(int id)
    {
        var category = await _foodStoreDbContext.FoodCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (category is null) return CategoryErrors.NotFound;
        _foodStoreDbContext.Remove(category);
        await _foodStoreDbContext.SaveChangesAsync();
        return Result.Success;
    }

    public async Task<ErrorOr<IList<GetAllFoodCategories.Response>>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _foodStoreDbContext.FoodCategories
           .OrderBy(x => x.Name)
           .ProjectToType<GetAllFoodCategories.Response>().ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<GetFoodCategoryById.Response>> GetCategoryByIdAsync(int id)
    {
        var category = await _foodStoreDbContext.FoodCategories.Include(f=>f.Foods)
         .FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
            return CategoryErrors.NotFound;

        return category.Adapt<GetFoodCategoryById.Response>();
    }

    public async Task<ErrorOr<Success>> UpdateCategoryAsync(UpdateFoodCategory.Command request)
    {
       var category = await _foodStoreDbContext.FoodCategories.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (category is null)
            return CategoryErrors.NotFound;

        // Check for duplicate name
        bool exists = await _foodStoreDbContext.FoodCategories
            .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower() && c.Id != request.Id);
        if (exists)
            return CategoryErrors.DuplicateName(request.Name);

        // Update category
        category.Name = request.Name;
        await _foodStoreDbContext.SaveChangesAsync();
        return Result.Success;
    }
}

