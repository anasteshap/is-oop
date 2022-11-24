namespace Banks.Models;

public class Percent
{
    private const double MinPercentValue = 0;
    public Percent(double value)
    {
        if (value < MinPercentValue)
        {
            throw new Exception();
        }

        Value = value / 100;
    }

    public double Value { get; }
}