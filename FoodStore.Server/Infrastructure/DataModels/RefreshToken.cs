namespace FoodStore.Server.Infrastructure.DataModels
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public required ApplicationUser User { get; set; }
        public required string UserId { get; set; }
    }
}
