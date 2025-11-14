using ErrorOr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodStore.Server.Domain.Valueobjects;

[ComplexType]
public class Email : ValueObject
{
    [Column("Email")]
    [MaxLength(50)]
    public string Value { get; init; }
    private Email(string value)
    {
        Value = value;
    }
    private Email() { }
    public static ErrorOr<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Error.Validation("Email.NullOrEmpty", "Email address can not be null or empty");
        if (!IsValidEmail(value))
            return Error.Validation("Email.InvalidFormat", "Email address is not in a valid state");
        return new Email(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    private static bool IsValidEmail(string email)
    {
        // Regular Expression
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Makes it case-insensitive
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
