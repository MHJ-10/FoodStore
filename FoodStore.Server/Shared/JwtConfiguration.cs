namespace FoodStore.Server.Settings;

public class JwtConfiguration
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required double ExpirationInMinutes { get; set; }
}
