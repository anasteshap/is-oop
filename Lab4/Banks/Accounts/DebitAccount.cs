using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DebitAccount : BaseAccount
{
    private readonly Percent _debitPercent;
    private readonly Limit _limitForDubiousClient;
    private readonly decimal _percentageAmount;
    internal DebitAccount(IClient client, BankConfiguration bankConfiguration)
        : base(client, TypeOfBankAccount.Debit)
    {
        _debitPercent = bankConfiguration.DebitPercent;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        _percentageAmount = 10000;
        _percentageAmount++;
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