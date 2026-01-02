using FoodStore.Server.Identity.DataModels;
using FoodStore.Server.Identity.FluentApi;
using FoodStore.Server.Infrastructure.FluentApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Server.Identity;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(builder);
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
