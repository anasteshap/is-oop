using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class DepositPercent
{
    public DepositPercent(Percent percent, decimal leftBorder, decimal rightBorder = decimal.Zero)
    {
        Percent = percent;
        LeftBorder = leftBorder;
        RightBorder = rightBorder != decimal.Zero ? rightBorder : decimal.MaxValue;
        if (leftBorder >= rightBorder)
            throw new Exception();
    }

    public Percent Percent { get; private set; }
    public decimal LeftBorder { get; }
    public decimal RightBorder { get; }

    public void SetPercent(decimal percent) => Percent = new Percent(percent);
}