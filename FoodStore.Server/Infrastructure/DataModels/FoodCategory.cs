using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.DataModels;

public class FoodCategory
{
    public int Id { get; set; }

    public required string Name { get; set; }
    // Navigation Property
    public ICollection<Food>? Foods { get; set; }
}
