using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DebitAccount : BaseAccount
{
    private readonly Percent _debitPercent;
    private readonly Limit _limitForDubiousClient;
    private readonly decimal _percentageAmount = 0;
    internal DebitAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
        : base(client, amount)
    {
        _debitPercent = bankConfiguration.DebitPercent;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
    }

    public override void IncreaseAmount(decimal sum)
    {
        if (sum <= 0)
        {
            throw new Exception();
        }

        Amount += sum;
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

        if (Client.IsDubious())
        {
            if (sum < _limitForDubiousClient.Value)
            {
                throw new Exception();
            }
        }

        Amount -= sum;
    }
}