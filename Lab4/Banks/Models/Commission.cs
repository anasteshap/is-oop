namespace Banks.Models;

public class Commission
{
    public Commission(double value)
    {
        if (value < 0)
        {
            throw new Exception();
        }

        Value = value / 100;
    }

    public double Value { get; }
}