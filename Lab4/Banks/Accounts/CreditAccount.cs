using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class CreditAccount : BaseAccount
{
    private readonly Commission _creditCommission;
    private readonly Limit _creditLimit;
    private readonly Limit _limitForDubiousClient;
    internal CreditAccount(IClient client, BankConfiguration bankConfiguration)
        : base(client, TypeOfBankAccount.Credit)
    {
        _creditCommission = bankConfiguration.CreditCommission;
        _creditLimit = bankConfiguration.CreditLimit;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
        {
            throw new Exception();
        }
    }
}