using Banks.Accounts.AccountConfigurations;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class CreditAccount : BaseAccount
{
    private readonly CreditAccountConfiguration _configuration;
    private readonly Limit _limitForDubiousClient;

    internal CreditAccount(IClient client, BankConfiguration bankConfiguration)
        : base(client, TypeOfBankAccount.Credit)
    {
        ArgumentNullException.ThrowIfNull(nameof(bankConfiguration));
        _configuration = bankConfiguration.CreditAccountConfiguration;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        Balance = bankConfiguration.CreditAccountConfiguration.CreditLimit;
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw TransactionException.NegativeAmount();

        if (Client.IsDubious)
        {
            if (sum > _limitForDubiousClient.Value)
                throw TransactionException.SumExceedingLimit(sum, _limitForDubiousClient.Value);
        }

        if (sum > Balance)
        {
            if (Math.Abs(Balance - sum - _configuration.CreditCommission) < _configuration.CreditLimit)
                throw AccountException.NotEnoughMoney();
        }

        Balance -= (sum <= Balance) ? sum : sum + _configuration.CreditCommission;
    }

    public override void AccountDailyPayoff() { }
}