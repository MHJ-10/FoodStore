using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using FoodStore.Server.Application.Foods.Commands;
using FoodStore.Server.Application.Foods.Error;
using FoodStore.Server.Application.Foods.Queries;
using FoodStore.Server.Infrastructure;
using FoodStore.Server.Infrastructure.DataModels;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Application.Services;

public class FoodService : IFoodService
{
    private readonly FoodStoreDbContext _db;
    public FoodService(FoodStoreDbContext db, IValidator<CreateFood.CreateFoodCommand> validator)
    {
        _db = db;
    }

    public async Task<ErrorOr<Unit>> AddFoodAsync(CreateFood.CreateFoodCommand food)
    {
        await _db.Foods.AddAsync(food.Adapt<Food>());
        await _db.SaveChangesAsync();
        return Unit.Value;
    }

    public async Task<ErrorOr<FindFood.Response>> FindFoodAsync(int foodId, CancellationToken cancellationToken)
    {
        var findedFood = await _db.Foods.AsNoTracking().Include(f => f.Category).FirstOrDefaultAsync(x => x.Id == foodId, cancellationToken);
        if (findedFood is null)
        {
            return FoodErrors.FoodNotFound(foodId);
        }
        return findedFood.Adapt<FindFood.Response>();
    }

    public async Task<ErrorOr<IList<GetAllFoods.Response>>> GetAllFoodsWithCategoriesAsync(CancellationToken cancellationToken)
    {
        var foods = await _db.Foods.Include(f => f.Category).ProjectToType<GetAllFoods.Response>().ToListAsync(cancellationToken);
        if (!foods.Any())
        {
            return FoodErrors.NoFoodsAvaliable;
        }
        return foods;
    }
}
