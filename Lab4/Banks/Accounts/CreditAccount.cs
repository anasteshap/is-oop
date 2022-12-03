using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class CreditAccount : BaseAccount
{
    private readonly CreditAccountConfiguration _configuration;
    private readonly Limit _limitForDubiousClient;

    // private readonly Limit _commissionAmount = 0;
    internal CreditAccount(IClient client, BankConfiguration bankConfiguration)
        : base(client, TypeOfBankAccount.Credit)
    {
        _configuration = bankConfiguration.CreditAccountConfiguration;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        Balance = bankConfiguration.CreditAccountConfiguration.CreditLimit;
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw new Exception("Sum can't be <= 0");

        if (Client.IsDubious)
        {
            if (sum > _limitForDubiousClient.Value)
                throw new Exception($"You can't withdraw money more than limit {_limitForDubiousClient.Value}");
        }

        if (sum > Balance)
        {
            if (Math.Abs(Balance - sum - _configuration.CreditCommission) < _configuration.CreditLimit)
                throw new Exception($"Not enough money");
        }

        Balance -= (sum <= Balance) ? sum : sum + _configuration.CreditCommission;
    }
}