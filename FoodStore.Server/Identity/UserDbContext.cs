using FoodStore.Server.Identity.DataModels;
using FoodStore.Server.Identity.FluentApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Server.Identity;

public class UserDbContext : IdentityDbContext<ApplicationUser>
{

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(builder);
    }
}
