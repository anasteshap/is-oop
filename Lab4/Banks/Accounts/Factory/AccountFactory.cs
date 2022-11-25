using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;

namespace Banks.Accounts.Factory;

internal class AccountFactory
{
    internal static BaseAccount CreateCreditAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return new CreditAccount(client, amount, bankConfiguration);
    }

    internal static BaseAccount CreateDebitAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return new DebitAccount(client, amount, bankConfiguration);
    }

    internal static BaseAccount CreateDepositAccount(IClient client, decimal amount, BankConfiguration bankConfiguration, uint? periodInDays)
    {
        return new DepositAccount(client, amount, bankConfiguration, periodInDays);
    }
}