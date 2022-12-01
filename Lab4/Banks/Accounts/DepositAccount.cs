using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DepositAccount : BaseAccount
{
    private readonly Dictionary<Range, Percent> _depositPercents;
    private readonly Limit _limitForDubiousClient;
    private readonly uint _depositPeriodInDays;
    internal DepositAccount(IClient client, BankConfiguration bankConfiguration, uint? periodInDays = null)
        : base(client, TypeOfBankAccount.Deposit)
    {
        _depositPercents = bankConfiguration.DepositPercents;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        _depositPeriodInDays = periodInDays ?? bankConfiguration.DepositPeriodInDays;
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