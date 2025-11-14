using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.FluentApi;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => new { x.OrderId, x.FoodId });
        builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Food).WithMany().HasForeignKey(x => x.FoodId);
        builder.OwnsOne(x => x.UnitPrice, money =>
        {
            money.Property(x => x.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2).IsRequired();
            money.Property(x => x.Currency).HasColumnName("Currency").IsRequired();
        });
        builder.ComplexProperty(x => x.Quantity).IsRequired();
    }
}
