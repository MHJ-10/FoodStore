namespace FoodStore.Server.Infrastructure.DataModels
{
    public class AddRole
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}