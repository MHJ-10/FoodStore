using ErrorOr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodStore.Server.Domain.Valueobjects;
[ComplexType]
public class PhoneNumber :  ValueObject
{
    [Column("PhoneNumber")]
    public string Value { get; init; }
    private PhoneNumber(string value)
    {
        Value = value;
    }
    private PhoneNumber() { } // EF Core requires a parameterless constructor

    public static ErrorOr<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation("PhoneNumber.Empty", "Phone number cannot be empty.");

        // Normalize
        value = value.Replace(" ", "").Replace("-", "");

        if (!IsValid(value))
            return Error.Validation("PhoneNumber.Invalid", "Invalid phone number format. It must contain 10–15 digits and optionally a + sign.");

        return new PhoneNumber(value);
    }
    public static bool IsValid(string value)
    {
        // Accepts +CountryCode and 10–15 digits
        var pattern = @"^\+?[0-9]{10,15}$";
        return Regex.IsMatch(value, pattern);
    }
    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
       yield return Value;
    }
}
