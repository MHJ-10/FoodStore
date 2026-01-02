using ErrorOr;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace FoodStore.Server.Domain.Valueobjects;

[ComplexType]
public class Username : ValueObject
{
    [Column("Username")]
    public string Value { get; init; }
    private Username(string value)
    {
        Value = value;
    }
    private Username() { }
    public static ErrorOr<Username> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Error.Validation("Username.NullOrEmpty", "Username can not be null or empty");
        if (!IsValid(value))
            return Error.Validation("Username.InvalidFormat", "Username must be 3-20 chars, start with a letter, and contain only letters, numbers, . or _.");
        return new Username(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    private static bool IsValid(string password)
    {
        var pattern = @"^(?!.*[._]{2})[a-zA-Z][a-zA-Z0-9._]{2,19}$";
        return Regex.IsMatch(password, pattern);
    }
}

