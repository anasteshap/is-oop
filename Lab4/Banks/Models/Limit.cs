using Banks.Exceptions;

namespace Banks.Models;

public class Limit
{
    public Limit(decimal limit)
    {
        if (limit < 0)
            throw TransactionException.NegativeAmount();

        Value = limit;
    }

    public decimal Value { get; }
}