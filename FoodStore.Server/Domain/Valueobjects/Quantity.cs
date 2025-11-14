using ErrorOr;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Domain.Valueobjects;

[ComplexType]
public class Quantity : ValueObject
{
    [Column("Quantity")]
    [ConcurrencyCheck]
    public int Value { get; init; }

    private Quantity(int value)
    {
        Value = value;
    }
    private Quantity() { } // EF Core requires a parameterless constructor
    public static ErrorOr<Quantity> Create(int value)
    {
        if (value <= 0)
            Error.Validation("Quantity.NegetiveValue", "Quantity must be greater than zero.");
        if (value > int.MaxValue)
            Error.Validation("Quantity.Value", $"Quantity must be less than {int.MaxValue}.");
        return new Quantity(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
