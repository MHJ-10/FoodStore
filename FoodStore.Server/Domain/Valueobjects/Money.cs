using ErrorOr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Domain.Valueobjects;

public class Money : ValueObject
{
    public decimal Amount { get; init; }
    [MaxLength(10)]
    public string Currency { get; init; }
    private Money(decimal amount, string currency = "USD")
    {
        Amount = amount;
        Currency = currency;
    }
    private Money() { } // EF Core needs a parameterless constructor

    public static ErrorOr<Money> Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            return Error.Validation("Money.NegativeAmount", "Money amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            return Error.Validation("Money.CurrencyRequired", "Currency is required.");

        if (currency.Length > 10)
            return Error.Validation("Money.CurrencyTooLong", "Currency cannot exceed 10 characters.");

        return new Money(amount, currency);
    }

    public static ErrorOr<Money> operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            return Error.Conflict("Money.CurrencyMismatch", "Cannot add money values with different currencies.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static ErrorOr<Money> operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            return Error.Conflict("Money.CurrencyMismatch", "Cannot subtract money values with different currencies.");

        return new Money(Math.Abs(a.Amount - b.Amount), a.Currency);
    }

    public static Money operator *(Money money, int multiplier)
        => new Money(money.Amount * multiplier, money.Currency);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return new object[] { Amount, Currency };
    }
}
