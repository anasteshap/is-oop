using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DepositAccount : BaseAccount
{
    private readonly Percent _depositPercent;
    private readonly Limit _limitForDubiousClient;
    internal DepositAccount(IClient client, decimal amount, BankConfiguration bankConfiguration1)
        : base(client, amount)
    {
        _depositPercent = bankConfiguration1.DepositPercent;
        _limitForDubiousClient = bankConfiguration1.LimitForDubiousClient;
    }

    public override void IncreaseAmount(decimal sum)
    {
        throw new NotImplementedException();
    }

    public override void DecreaseAmount(decimal sum)
    {
        throw new NotImplementedException();
    }
}