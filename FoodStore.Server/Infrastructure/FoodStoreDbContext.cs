using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Infrastructure.FluentApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FoodStore.Server.Infrastructure;

public class FoodStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public FoodStoreDbContext(DbContextOptions<FoodStoreDbContext> options)
        : base(options)
    {
    }
    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new FoodConfiguration());
        modelBuilder.ApplyConfiguration(new FoodCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
