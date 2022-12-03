using Banks.Exceptions;
using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class DepositPercent
{
    public DepositPercent(Percent percent, decimal leftBorder, decimal rightBorder = decimal.Zero)
    {
        ArgumentNullException.ThrowIfNull(nameof(percent));
        Percent = percent;
        if (leftBorder < 0 || rightBorder < 0)
            throw AccountException.InvalidConfiguration("DepositPercent: leftBorder or rightBorder < 0");
        LeftBorder = leftBorder;
        RightBorder = rightBorder != decimal.Zero ? rightBorder : decimal.MaxValue;
        if (leftBorder >= rightBorder)
            throw AccountException.InvalidConfiguration("DepositPercent: LeftBorder >= rightBorder");
    }

    public Percent Percent { get; }
    public decimal LeftBorder { get; }
    public decimal RightBorder { get; }
}