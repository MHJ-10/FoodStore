using ErrorOr;

namespace FoodStore.Server.Application.FoodCategory.Errors;

public static class CategoryErrors
{
    public static Error DuplicateName(string name) =>
        Error.Conflict(
            code: "Category.DuplicateName",
            description: $"Category with name '{name}' already exists.");

    public static readonly Error NotFound =
        Error.NotFound(
            code: "Category.NotFound",
            description: "Category not found.");

    public static readonly Error FailedToCreate =
        Error.Failure(
            code: "Category.CreateFailed",
            description: "An error occurred while creating the category.");
}
