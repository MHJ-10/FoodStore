using Microsoft.AspNetCore.Identity;


namespace FoodStore.Server.Identity.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
    }
}


