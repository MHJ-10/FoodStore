using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Identity.DataModels;

namespace FoodStore.Server.Infrastructure.DataModels;

public class Customer
{
    public int Id { get; set; }

    // Optional link to Identity user (AspNetUsers.Id)
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Navigation Property
    public ICollection<Order>? Orders { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public required Email Email { get; set; }

    public PhoneNumber? PhoneNumber { get; set; }

    public string? Address { get; set; }
}
