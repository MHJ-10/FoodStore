using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodStore.Server.Infrastructure.Persistence;

public class FoodStoreDbContextFactory
    : IDesignTimeDbContextFactory<FoodStoreDbContext>
{
    public FoodStoreDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FoodStoreDbContext>();

        var connectionString = configuration.GetConnectionString("FoodStoreConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new FoodStoreDbContext(optionsBuilder.Options);
    }
}
