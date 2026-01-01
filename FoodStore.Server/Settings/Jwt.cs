namespace FoodStore.Server.Settings
{
    public class Jwt
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required double DurationInMinutes { get; set; }
    }
}
