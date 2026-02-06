using ErrorOr;
using System.Text.RegularExpressions;

namespace FoodStore.Server.Domain.Valueobjects;

public class Password : ValueObject
{
    public string Value { get; init; }

    private Password(string value)
    {
        Value = value;
    }
    public static ErrorOr<Password> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Error.Validation("Password.NullOrEmpty", "Password cannot be null or empty");

        if (!IsValid(value))
            return Error.Validation("Password.InvalidFormat", "Password must be 8+ characters with uppercase, lowercase, and a number.");

        return new Password(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static bool IsValid(string password)
    {
        var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$_#!*%&(). ]{8,}$";
        return Regex.IsMatch(password, pattern);
    }
}
