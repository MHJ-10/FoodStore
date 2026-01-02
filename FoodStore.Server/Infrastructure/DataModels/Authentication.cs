namespace FoodStore.Server.Infrastructure.DataModels
{
    public class Authentication
    {
        public string? Message { get; set; }
        public string? Username { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
