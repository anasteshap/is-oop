using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class CreditAccount : BaseAccount
{
    private readonly Commission _creditCommission;
    private readonly Limit _creditLimit;
    private readonly Limit _limitForDubiousClient;
    internal CreditAccount(IClient client, decimal amount, BankConfiguration bankConfiguration1)
        : base(client, amount)
    {
        _creditCommission = bankConfiguration1.CreditCommission;
        _creditLimit = bankConfiguration1.CreditLimit;
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