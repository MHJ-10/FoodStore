using FoodStore.Server.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FoodStore.Server.Identity.DataModels
{
    public class ApplicationUser : IdentityUser,ISoftDeletable
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}


