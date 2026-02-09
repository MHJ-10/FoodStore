using FoodStore.Server.Domain.Valueobjects;

namespace FoodStore.Server.Presentation.ViewModels;

public class FoodViewModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public Money Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    public byte[]? FoodImage { get; set; }
}
