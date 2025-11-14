using FoodStore.Server.Domain.Valueobjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.DataModels;

public class Food
{
    public int Id { get; set; }

    // Foreign Key
    public int FoodCategoryId { get; set; }
    // Navigation Property
    public required FoodCategory Category { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public Money Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    public byte[]? FoodImage { get; set; }
}
