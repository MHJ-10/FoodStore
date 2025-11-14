using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.FluentApi;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(300);
        builder.OwnsOne(x => x.Price, money =>
        {
            money.Property(x => x.Amount).HasColumnName("Price").HasPrecision(18, 2).IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.HasOne(x => x.Category).WithMany(x => x.Foods).HasForeignKey(x => x.FoodCategoryId);
    }
}
