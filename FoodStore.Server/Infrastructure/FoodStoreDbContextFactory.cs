using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure;

public class FoodStoreDbContextFactory : IDesignTimeDbContextFactory<FoodStoreDbContext>
{
    public FoodStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FoodStoreDbContext>();

        optionsBuilder.UseSqlServer("Server=.;Database=FoodStoreDb;Trusted_Connection=True;TrustServerCertificate=True");

        return new FoodStoreDbContext(optionsBuilder.Options);
    }
}
