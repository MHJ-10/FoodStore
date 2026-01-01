using ErrorOr;
using FluentValidation;
using FoodStore.Server.Application.Foods.Error;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Application.Foods.Commands;

public static class CreateFood
{
    public class CreateFoodCommand : IRequest<ErrorOr<Unit>>
    {
        public int FoodCategoryId { get; set; }
        public required string Name { get; set; }

        public string? Description { get; set; }

        public Money Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public byte[]? FoodImage { get; set; }
    }
    public sealed class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
    {
        public CreateFoodCommandValidator()
        {
            RuleFor(food => food.Name).MaximumLength(50).MinimumLength(3).NotEmpty().WithState(_ => FoodErrors.NameRequired);
            RuleFor(food => food.Description).MaximumLength(300).WithState(_=>FoodErrors.DescriptionTooLong);
            RuleFor(food => food.FoodImage)
             .Must(image => image == null || image.Length <= 5 * 1024 * 1024)
             .WithState(_=>FoodErrors.ImageTooLarge);
            RuleFor(food => food.FoodCategoryId).NotNull().NotEqual(0).WithState(_=>FoodErrors.CategoryRequired);
        }
    }
    public class Handler : IRequestHandler<CreateFoodCommand, ErrorOr<Unit>>
    {
        private readonly IFoodService _foodService;
        public Handler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public async Task<ErrorOr<Unit>> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
        {
            var result = await _foodService.AddFoodAsync(request);
            return result;
        }

    }

}
