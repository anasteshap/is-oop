using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DepositAccount : BaseAccount
{
    private readonly Percent _depositPercent;
    private readonly Limit _limitForDubiousClient;
    private readonly uint _depositPeriodInDays;
    internal DepositAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
        : base(client, amount)
    {
        _depositPercent = bankConfiguration.DepositPercent;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        _depositPeriodInDays = bankConfiguration.DepositPeriodInDays;
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
        {
            throw new Exception();
        }

        if (Amount < sum)
        {
            throw new Exception();
        }

        Amount -= sum;
    }
}