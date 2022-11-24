namespace Banks.Models;

public class Limit
{
    public Limit(decimal limit)
    {
        if (limit < 0)
        {
            throw new Exception();
        }

        Value = limit;
    }

    public decimal Value { get; }
}