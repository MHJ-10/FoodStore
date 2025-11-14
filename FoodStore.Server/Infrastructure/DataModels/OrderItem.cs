using FoodStore.Server.Domain.Valueobjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.DataModels;

public class OrderItem
{
    // Composite Key (OrderId + FoodId)
    public int OrderId { get; set; }
    public int FoodId { get; set; }
    public required Food Food { get; set; }
    public required Order Order { get; set; }
    public required Quantity Quantity { get; set; }
    public required Money UnitPrice { get; set; }
}
