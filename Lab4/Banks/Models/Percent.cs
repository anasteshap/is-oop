using Banks.Exceptions;

namespace Banks.Models;

public class Percent
{
    private const decimal MinPercentValue = 0;
    public Percent(decimal value)
    {
        if (value < MinPercentValue)
            throw TransactionException.NegativeAmount();

        Value = value / 100;
    }

    public decimal Value { get; }
}