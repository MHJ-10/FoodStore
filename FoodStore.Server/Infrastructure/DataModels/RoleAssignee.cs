namespace FoodStore.Server.Infrastructure.DataModels
{
    public class RoleAssignee
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}