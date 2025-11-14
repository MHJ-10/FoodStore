using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.DataModels;

public class Order
{
    public int Id { get; set; }
    // Foreign Key
    public int CustomerId { get; set; }
    // Navigation property
    public required Customer Customer { get; set; }

    public DateTime OrderDate { get; set; }

    public Money TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    // Many-to-Many (via OrderItem)
    public required ICollection<OrderItem> OrderItems { get; set; }
}
