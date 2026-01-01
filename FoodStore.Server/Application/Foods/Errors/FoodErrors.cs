namespace FoodStore.Server.Application.Foods.Error;

using ErrorOr;
public static class FoodErrors
{
    public static Error NameRequired =>
        Error.Validation(
            code: "Food.Name.Required",
            description: "Food name is required");

    public static Error NameLength =>
        Error.Validation(
            code: "Food.Name.Length",
            description: "Food name must be between 3 and 50 characters");

    public static Error DescriptionTooLong =>
        Error.Validation(
            code: "Food.Description.Length",
            description: "Food description cannot exceed 300 characters");

    public static Error ImageTooLarge =>
        Error.Validation(
            code: "Food.Image.Size",
            description: "Food image size cannot exceed 5 MB");

    public static Error CategoryRequired =>
        Error.Validation(
            code: "Food.Category.Required",
            description: "Food category must be specified");
    public static Error FoodNotFound(int foodId) =>
        Error.NotFound(
            code: "Food.NotFound",
            description: $"Food with ID {foodId} was not found");
    public static Error NoFoodsAvaliable =>
        Error.NotFound(
            code: "Food.NoFoodsAvailable",
            description: "No foods are available in the store");


}
