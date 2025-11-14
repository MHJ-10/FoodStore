using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Infrastructure.FluentApi;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Address).HasMaxLength(200);
        builder.Property(x => x.FirstName).HasMaxLength(10).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(20).IsRequired();
        builder.HasMany(x => x.Orders).WithOne(x => x.Customer);
        builder.ComplexProperty(x => x.Email).IsRequired();
        builder.ComplexProperty(x => x.PhoneNumber,
            phone =>
            {
                phone.Property(x => x.Value).HasMaxLength(50).IsRequired();
            });
    }
}
