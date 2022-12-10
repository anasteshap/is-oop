using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class DebitAccountConfiguration
{
    public DebitAccountConfiguration(Percent debitPercent)
    {
        ArgumentNullException.ThrowIfNull(nameof(debitPercent));
        DebitPercent = debitPercent;
    }

    public Percent DebitPercent { get; private set; }
    public void SetDebitPercent(decimal percent) => DebitPercent = new Percent(percent);
}