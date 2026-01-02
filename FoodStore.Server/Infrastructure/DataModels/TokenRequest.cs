namespace FoodStore.Server.Infrastructure.DataModels
{
    public class TokenRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
